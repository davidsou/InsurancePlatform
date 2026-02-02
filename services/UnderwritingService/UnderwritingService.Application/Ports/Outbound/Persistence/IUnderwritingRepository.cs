using UnderwritingService.Domain;

namespace UnderwritingService.Application.Ports.Outbound.Persistence;

public interface IUnderwritingRepository
{
    Task AddAsync(Underwriting underwriting, CancellationToken ct);
    Task<Underwriting?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Underwriting?> GetByProposalIdAsync(Guid proposalId, CancellationToken ct);
    Task<bool> ExistsByProposalIdAsync(Guid proposalId, CancellationToken ct);
    Task<IReadOnlyList<Underwriting>> ListAsync(CancellationToken ct);
}
