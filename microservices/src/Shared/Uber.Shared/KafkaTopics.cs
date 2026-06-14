using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Shared;

public static class KafkaTopics
{
    // Domain events — published by services via outbox
    public const string VoyageRequested = "voyage.requested";
    public const string TripStarted = "trip.started";
    public const string TripCompleted = "trip.completed";
    public const string TripCancelled = "trip.cancelled";

    // Events from other services — consumed by Trip saga
    public const string DriverFound = "driver.found";
    public const string NoDriverFound = "driver.not-found";
    public const string DriverAccepted = "driver.accepted";
    public const string DriverCancelled = "driver.cancelled";
    public const string PaymentProcessed = "payment.processed";
    public const string PaymentFailed = "payment.failed";

    // Commands — published by saga, consumed by other services
    public const string FindDriver = "cmd.find-driver";
    public const string LockDriver = "cmd.lock-driver";
    public const string ReleaseDriver = "cmd.release-driver";
    public const string NotifyPassenger = "cmd.notify-passenger";
    public const string OpenBillingSession = "cmd.open-billing-session";
    public const string ProcessPayment = "cmd.process-payment";
    public const string CancelTripCommand = "cmd.cancel-trip";
}