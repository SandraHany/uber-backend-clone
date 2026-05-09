using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Trip.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
