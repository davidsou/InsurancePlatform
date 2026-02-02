using Microsoft.EntityFrameworkCore;
using ProposalService.Application.Ports.Outbound.Persistence;
using ProposalService.Domain;

namespace ProposalService.Infrastructure.Persistence;

public sealed class EfProposalRepository : IProposalRepository
{
    private readonly ProposalDbContext _db;

    public EfProposalRepository(ProposalDbContext db) => _db = db;

    public Task AddAsync(Proposal proposal, CancellationToken ct)
        => _db.Proposals.AddAsync(proposal, ct).AsTask();

    public Task<Proposal?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Proposals.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Proposal>> ListAsync(CancellationToken ct)
        => await _db.Proposals.OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);
}
