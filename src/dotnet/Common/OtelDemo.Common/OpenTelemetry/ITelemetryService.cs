namespace OtelDemo.Common.OpenTelemetry;

public interface ITelemetryService : IDisposable
{
    ITelemetryService AddTag(string tag, object? value);
    ITelemetryService AddInformationEvent(string message);
    ITelemetryService AddLogInformationAndEvent(string messageTemplate, object? propertyValues);
    ITelemetryService AddWarningEvent(string messageTemplate, object? propertyValues);
    ITelemetryService AddException(string error, Exception exception);
    void SetSucess(string messageTemplate, object? propertyValues);
    void SetError(string messageTemplate, object? propertyValues);
    void AddWarning(string messageTemplate, params object?[]? propertyValues);
    void AddInformation(string messageTemplate, params object?[]? propertyValues);
}