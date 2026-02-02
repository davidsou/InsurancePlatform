using Microsoft.EntityFrameworkCore;
using UnderwritingService.Application.Ports.Outbound.Persistence;
using UnderwritingService.Domain;

namespace UnderwritingService.Infrastructure.Persistence;

public sealed class EfUnderwritingRepository : IUnderwritingRepository
{
    private readonly UnderwritingDbContext _db;

    public EfUnderwritingRepository(UnderwritingDbContext db) => _db = db;

    public Task AddAsync(Underwriting underwriting, CancellationToken ct)
        => _db.Underwritings.AddAsync(underwriting, ct).AsTask();

    public Task<Underwriting?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Underwritings.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Underwriting?> GetByProposalIdAsync(Guid proposalId, CancellationToken ct)
        => _db.Underwritings.FirstOrDefaultAsync(x => x.ProposalId == proposalId, ct);

    public Task<bool> ExistsByProposalIdAsync(Guid proposalId, CancellationToken ct)
        => _db.Underwritings.AnyAsync(x => x.ProposalId == proposalId, ct);

    public async Task<IReadOnlyList<Underwriting>> ListAsync(CancellationToken ct)
        => await _db.Underwritings.OrderByDescending(x => x.ContractedAtUtc).ToListAsync(ct);
}
