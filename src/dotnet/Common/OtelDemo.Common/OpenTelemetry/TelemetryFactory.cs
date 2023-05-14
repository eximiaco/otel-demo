using Serilog;

namespace OtelDemo.Common.OpenTelemetry;

public interface ITelemetryFactory
{
    ITelemetryService Create(string spanName);
}

public class TelemetryFactory : ITelemetryFactory
{
    private readonly OtelTracingService _otelTracingService;
    private readonly ILogger _logger;

    public TelemetryFactory(
        OtelTracingService otelTracingService,
        ILogger logger)
    {
        _otelTracingService = otelTracingService;
        _logger = logger;
    }
        
    public ITelemetryService Create(string spanName)
    {
        return new TelemetryService(
            _otelTracingService.ActivitySource.StartActivity(spanName),
            _logger);
    }
}