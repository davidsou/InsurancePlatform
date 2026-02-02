using Insurance.SharedKernel;

namespace UnderwritingService.Application.Ports.Inbound.Underwritings;

public interface IGetByProposalUseCase
{
    Task<OperationResult<UnderwritingDto>> ExecuteAsync(Guid proposalId, CancellationToken ct);
}
