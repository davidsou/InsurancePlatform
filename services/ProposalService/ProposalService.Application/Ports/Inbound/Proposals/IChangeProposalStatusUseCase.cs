using Insurance.SharedKernel;

namespace ProposalService.Application.Ports.Inbound.Proposals;

public interface IChangeProposalStatusUseCase
{
    Task<OperationResult> ExecuteAsync(Guid proposalId, ChangeProposalStatusRequest request, CancellationToken ct);
}
