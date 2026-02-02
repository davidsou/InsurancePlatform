using Insurance.SharedKernel;
using ProposalService.Application.Ports.Inbound.Proposals;
using ProposalService.Application.Ports.Outbound.Persistence;

namespace ProposalService.Application.UseCases.Proposals;

public sealed class GetProposalUseCase : IGetProposalUseCase
{
    private readonly IProposalRepository _repo;

    public GetProposalUseCase(IProposalRepository repo) => _repo = repo;

    public async Task<OperationResult<ProposalDto>> ExecuteAsync(Guid proposalId, CancellationToken ct)
    {
        var proposal = await _repo.GetByIdAsync(proposalId, ct);
        if (proposal is null)
            return Errors.NotFound<ProposalDto>("Proposal", proposalId.ToString());

        return OperationResult<ProposalDto>.Ok(new ProposalDto(
            proposal.Id,
            proposal.CustomerName,
            proposal.ProductCode,
            proposal.CoverageAmount,
            proposal.Status.ToString(),
            proposal.CreatedAtUtc
        ));
    }
}
