using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Voyage.Domain.Enums;

public enum VoyageStatus
{
    Requested = 1,
    SearchingForDriver = 2,
    DriverFound = 3,
    InProgress = 4,
    Completed = 5,
    Cancelled = 6,
    Failed = 7
}
