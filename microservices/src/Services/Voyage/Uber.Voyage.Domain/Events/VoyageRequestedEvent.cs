using System;
using System.Collections.Generic;
using System.Text;
using Uber.Shared.Primitives;

namespace Uber.Voyage.Domain.Events;

public sealed record VoyageRequestedDomainEvent(Guid TripId, Guid RiderId) : DomainEvent;
