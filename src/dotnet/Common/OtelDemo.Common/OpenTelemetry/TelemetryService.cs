using System.Diagnostics;
using System.Reflection;
using Serilog;
using Serilog.Context;
using SmartFormat;

namespace OtelDemo.Common.OpenTelemetry;

public sealed class TelemetryService : ITelemetryService
{
    private readonly Activity? _activity;
    private readonly ILogger _logger;
    private bool _statusSet = false;
    private IList<IDisposable> _pushProperties = new List<IDisposable>();
    
    public TelemetryService(Activity? activity, ILogger logger)
    {
        _activity = activity;
        _logger = logger;
    }

    public ITelemetryService AddTag(string tag, object? value)
    {
        _activity?.SetTag(tag, value);
        _pushProperties.Add(LogContext.PushProperty(tag, value));
        return this;
    }

    public ITelemetryService AddInformationEvent(string message)
    {
        _activity?.AddEvent(new ActivityEvent(message));
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.Information(message);
        return this;
    }

    public ITelemetryService AddLogInformationAndEvent(string messageTemplate, object? propertyValues)
    {
        _activity?.AddEvent(new ActivityEvent(Smart.Format(messageTemplate, propertyValues!)));
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.Information(messageTemplate, propertyValues?
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(p => p.GetValue(propertyValues))
            .ToArray());
        return this;
    }

    public ITelemetryService AddWarningEvent(string messageTemplate, object? propertyValues)
    {
        _activity?.AddEvent(new ActivityEvent(Smart.Format(messageTemplate, propertyValues!)));
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.Warning(messageTemplate, propertyValues?
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(p => p.GetValue(propertyValues))
            .ToArray());
        return this;
    }
    
    public ITelemetryService AddException(string error, Exception exception)
    {
        _activity?.AddEvent(new ActivityEvent(exception.StackTrace!));
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.Fatal(exception, error);
        return this;
    }

    public void SetSucess(string messageTemplate, object? propertyValues)
    {
        _activity?.SetStatus(ActivityStatusCode.Ok, Smart.Format(messageTemplate, propertyValues!));
        _statusSet = true;
    }
    
    public void SetError(string messageTemplate, object? propertyValues)
    {
        _activity?.SetStatus(ActivityStatusCode.Error, Smart.Format(messageTemplate, propertyValues!));
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.Error(messageTemplate, propertyValues?
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(p => p.GetValue(propertyValues))
            .ToArray());
        _statusSet = true;
    }

    public void AddWarning(string messageTemplate, params object?[]? propertyValues)
    {
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.Warning(messageTemplate: messageTemplate, propertyValues);
    }
    
    public void AddInformation(string messageTemplate, params object?[]? propertyValues)
    {
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.Information(messageTemplate: messageTemplate, propertyValues);
    }
    
    public void Dispose()
    {
        if(!_statusSet)
            _activity?.SetStatus(ActivityStatusCode.Ok, "Process completed");

        foreach (var pushProperty in _pushProperties)
        {
            pushProperty.Dispose();
        }
        
        _activity?.Dispose();
    }
}