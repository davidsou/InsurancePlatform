using Insurance.SharedKernel;

namespace ProposalService.Application.Ports.Inbound.Proposals;

public interface IListProposalsUseCase
{
    Task<OperationResult<IReadOnlyList<ProposalDto>>> ExecuteAsync(CancellationToken ct);
}
