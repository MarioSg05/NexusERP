using Microsoft.EntityFrameworkCore;
using NexusERP.Domain.Customers.Aggregates;
using NexusERP.Domain.Identity.Aggregates;

namespace NexusERP.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}