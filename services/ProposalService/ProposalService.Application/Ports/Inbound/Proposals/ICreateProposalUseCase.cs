using Insurance.SharedKernel;

namespace ProposalService.Application.Ports.Inbound.Proposals;

public interface ICreateProposalUseCase
{
    Task<OperationResult<CreateProposalResponse>> ExecuteAsync(CreateProposalRequest request, CancellationToken ct);
}
