using ProposalService.Domain;

namespace ProposalService.Application.Ports.Outbound.Persistence;

public interface IProposalRepository
{
    Task AddAsync(Proposal proposal, CancellationToken ct);
    Task<Proposal?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Proposal>> ListAsync(CancellationToken ct);
}
