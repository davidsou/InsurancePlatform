using Insurance.SharedKernel;
using UnderwritingService.Application.Ports.Inbound.Underwritings;
using UnderwritingService.Application.Ports.Outbound.Persistence;

namespace UnderwritingService.Application.UseCases.Underwritings;

public sealed class GetByProposalUseCase : IGetByProposalUseCase
{
    private readonly IUnderwritingRepository _repo;

    public GetByProposalUseCase(IUnderwritingRepository repo) => _repo = repo;

    public async Task<OperationResult<UnderwritingDto>> ExecuteAsync(Guid proposalId, CancellationToken ct)
    {
        var uw = await _repo.GetByProposalIdAsync(proposalId, ct);
        if (uw is null)
            return Errors.NotFound<UnderwritingDto>("Underwriting (by ProposalId)", proposalId.ToString());

        return OperationResult<UnderwritingDto>.Ok(new UnderwritingDto(uw.Id, uw.ProposalId, uw.ContractedAtUtc));
    }
}
