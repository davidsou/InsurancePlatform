using Insurance.SharedKernel;

namespace UnderwritingService.Application.Ports.Inbound.Underwritings;

public interface IListUnderwritingsUseCase
{
    Task<OperationResult<IReadOnlyList<UnderwritingDto>>> ExecuteAsync(CancellationToken ct);
}
