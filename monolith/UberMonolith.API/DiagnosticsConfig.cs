using System.Diagnostics.Metrics;

namespace UberMonolith.API;

public static class DiagnosticsConfig
{
    public const string ServiceName = "UberMonolith";
    public static Meter Meter = new (ServiceName);
    public static Counter<int> TripRequestsCounter = Meter.CreateCounter<int>("triprequests.count");
}
