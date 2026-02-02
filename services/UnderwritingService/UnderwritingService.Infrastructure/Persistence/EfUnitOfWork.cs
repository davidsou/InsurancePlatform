using UnderwritingService.Application.Ports.Outbound.Persistence;

namespace UnderwritingService.Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly UnderwritingDbContext _db;

    public EfUnitOfWork(UnderwritingDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
