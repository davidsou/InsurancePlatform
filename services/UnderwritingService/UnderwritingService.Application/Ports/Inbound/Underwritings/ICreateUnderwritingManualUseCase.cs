using Insurance.SharedKernel;

namespace UnderwritingService.Application.Ports.Inbound.Underwritings;

public interface ICreateUnderwritingManualUseCase
{
    Task<OperationResult> ExecuteAsync(CreateUnderwritingManualRequest request, CancellationToken ct);
}
