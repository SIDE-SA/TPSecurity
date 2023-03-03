using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace TPSecurity.Api;

public class ThreadIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "TraceId", Activity.Current?.Id));
    }
}
