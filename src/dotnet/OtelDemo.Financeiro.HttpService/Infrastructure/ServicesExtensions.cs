using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.ServiceBus.Silverback;
using OtelDemo.Domain.InscricoesContext.Inscricoes.Eventos;
using OtelDemo.Inscricoes.InscricoesContext.Shared.Telemetria;
using Serilog;
using Serilog.Enrichers.OpenTelemetry;
using Serilog.Filters;
using Serilog.Sinks.OpenTelemetry;
using Silverback.Messaging.Configuration;

namespace OtelDemo.Financeiro.HttpService.Infrastructure;

internal static class ServicesExtensions
    {
        public static IServiceCollection AddTelemetry(this IServiceCollection serviceCollection, string serviceName,
            string serviceVersion, IConfiguration configuration)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            TelemetrySettings settings;
            if (configuration.GetSection("OpenTelemetry") is var section && section.Exists())
                settings = new TelemetrySettings(serviceName, serviceVersion,
                    new TelemetryExporter(section["Type"] ?? string.Empty, section["Endpoint"]?? string.Empty));
            else
                settings = new TelemetrySettings(serviceName, serviceVersion,
                    new TelemetryExporter("console", ""));
            var metrics = new OtelMetrics();
            
            serviceCollection.AddSingleton(settings);
            serviceCollection.AddScoped(sp => new OtelTracingService(sp.GetService<TelemetrySettings>()));
            serviceCollection.AddSingleton(metrics);
            serviceCollection.AddSingleton<OtelVariables>();
            
            Action<ResourceBuilder> configureResource = r => r.AddService(
                serviceName: settings.ServiceName,
                serviceVersion: settings.ServiceVersion,
                serviceInstanceId: Environment.MachineName);
            
            serviceCollection
                .AddOpenTelemetry()
                .ConfigureResource(configureResource)
                .WithTracing(builder =>
                {
                    builder
                        .AddSource(settings.ServiceName)
                        .AddSource("Silverback.Integration.Produce")
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation(o =>
                        {
                            o.RecordException = true;
                            o.SetDbStatementForText = true;
                        })
                        .AddEntityFrameworkCoreInstrumentation(o => { })
                        .AddAspNetCoreInstrumentation(opts =>
                        {
                            opts.EnrichWithHttpRequest = (a, r) => a?.AddTag("env", environmentName);
                            opts.RecordException = true;

                        })
                        .AddOtlpExporter(config =>
                        {
                            config.Endpoint = new Uri(settings.Exporter.Endpoint);
                        });
                })
                .WithMetrics(builder =>
                {
                    builder
                        .ConfigureResource(configureResource)
                        .AddMeter(metrics.Name)
                        .AddRuntimeInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddOtlpExporter(config =>
                        {
                            config.Endpoint = new Uri(settings.Exporter.Endpoint ?? string.Empty);
                        });
                });
            return serviceCollection;
        }
        
        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.ToString());
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    }
                );

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                );

                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Cdc Consumer",
                        Description = "Consumer consolidator worker.",
                        Version = "v1"
                    }
                );
            });
            return services;
        }
        
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(
                o =>
                    o.AddPolicy(
                        "default",
                        builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                        }
                    )
            );
            return services;
        }
        
        public static IServiceCollection AddHealth(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy(), new string[] { "ready" });
            // hcBuilder
            //     .AddNpgSql(
            //         configuration.GetConnectionString(Ambient.DatabaseConnectionName)!,
            //         name: "integration-store-check",
            //         tags: new string[] {"IntegrationStoreCheck", "health"});
            return services;
        }

        public static IServiceCollection AddWorkersServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
        
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            // services
            //     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddMicrosoftIdentityWebApi(configuration);
            return services;
        }
        
        public static IServiceCollection AddLogs(this IServiceCollection services, IConfiguration configuration, string serviceName)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithOpenTelemetrySpanId()
                .Enrich.WithOpenTelemetryTraceId()
                .Enrich.WithProperty("service_name",serviceName )
                //.Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Hosting.Diagnostics"))
                //.Filter.ByExcluding(Matching.FromSource("Microsoft.Hosting.Lifetime"))
                .Filter.ByExcluding(
                    Matching.FromSource("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager")
                )
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = "http://localhost:4317";
                    options.IncludedData = IncludedData.MessageTemplateTextAttribute |
                        IncludedData.TraceIdField | IncludedData.SpanIdField;
                })
                .CreateLogger();
            services.AddSingleton(Log.Logger);
            return services;
        }

        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();
            return services;
        }

        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection kafkaSection = configuration.GetSection("Kafka");
            var kafkaConfig = new KafkaConfig();
            kafkaConfig.Connection = kafkaSection.GetSection("Connection").Get<KafkaConnectionConfig>()!;
            services.AddSingleton(kafkaConfig);
            services
                .AddSilverback()
                .WithConnectionToMessageBroker(config => config.AddKafka())
                .AddKafkaEndpoints(endpoints => endpoints
                    .Configure(config => config.Configure(kafkaConfig))
                    .AddOutbound<InscricaoRealizadaEvento>(endpoint => endpoint
                        .ProduceTo("inscricoes")
                        .WithKafkaKey<InscricaoRealizadaEvento>(envelope => envelope.Message!.Id)
                        .SerializeAsJson(serializer => serializer.UseFixedType<InscricaoRealizadaEvento>())
                        .DisableMessageValidation()));
            return services;
        }
    }