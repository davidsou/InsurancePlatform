using ProposalService.Application.Ports.Outbound.Persistence;

namespace ProposalService.Infrastructure.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly ProposalDbContext _db;

    public EfUnitOfWork(ProposalDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
