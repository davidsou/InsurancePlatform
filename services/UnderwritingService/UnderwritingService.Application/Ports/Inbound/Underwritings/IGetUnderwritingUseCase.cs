using Insurance.SharedKernel;

namespace UnderwritingService.Application.Ports.Inbound.Underwritings;

public interface IGetUnderwritingUseCase
{
    Task<OperationResult<UnderwritingDto>> ExecuteAsync(Guid underwritingId, CancellationToken ct);
}
