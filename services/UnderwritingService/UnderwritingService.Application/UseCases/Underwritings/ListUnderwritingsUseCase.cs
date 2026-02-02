using Insurance.SharedKernel;
using UnderwritingService.Application.Ports.Inbound.Underwritings;
using UnderwritingService.Application.Ports.Outbound.Persistence;

namespace UnderwritingService.Application.UseCases.Underwritings;

public sealed class ListUnderwritingsUseCase : IListUnderwritingsUseCase
{
    private readonly IUnderwritingRepository _repo;

    public ListUnderwritingsUseCase(IUnderwritingRepository repo) => _repo = repo;

    public async Task<OperationResult<IReadOnlyList<UnderwritingDto>>> ExecuteAsync(CancellationToken ct)
    {
        var list = await _repo.ListAsync(ct);
        var result = list.Select(x => new UnderwritingDto(x.Id, x.ProposalId, x.ContractedAtUtc)).ToList().AsReadOnly();
        return OperationResult<IReadOnlyList<UnderwritingDto>>.Ok(result);
    }
}
