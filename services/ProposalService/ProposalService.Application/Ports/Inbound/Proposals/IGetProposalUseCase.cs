using Insurance.SharedKernel;

namespace ProposalService.Application.Ports.Inbound.Proposals;

public interface IGetProposalUseCase
{
    Task<OperationResult<ProposalDto>> ExecuteAsync(Guid proposalId, CancellationToken ct);
}
