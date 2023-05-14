using System.Diagnostics;

namespace OtelDemo.Common.OpenTelemetry;

public sealed class OtelTracingService
{
    public ActivitySource ActivitySource { get; }

    public OtelTracingService(TelemetrySettings? settings) => ActivitySource = new(settings!.ServiceName);
}