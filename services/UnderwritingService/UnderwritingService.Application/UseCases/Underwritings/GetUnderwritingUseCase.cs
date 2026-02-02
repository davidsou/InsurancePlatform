using Insurance.SharedKernel;
using UnderwritingService.Application.Ports.Inbound.Underwritings;
using UnderwritingService.Application.Ports.Outbound.Persistence;

namespace UnderwritingService.Application.UseCases.Underwritings;

public sealed class GetUnderwritingUseCase : IGetUnderwritingUseCase
{
    private readonly IUnderwritingRepository _repo;

    public GetUnderwritingUseCase(IUnderwritingRepository repo) => _repo = repo;

    public async Task<OperationResult<UnderwritingDto>> ExecuteAsync(Guid underwritingId, CancellationToken ct)
    {
        var uw = await _repo.GetByIdAsync(underwritingId, ct);
        if (uw is null)
            return Errors.NotFound<UnderwritingDto>("Underwriting", underwritingId.ToString());

        return OperationResult<UnderwritingDto>.Ok(new UnderwritingDto(uw.Id, uw.ProposalId, uw.ContractedAtUtc));
    }
}
