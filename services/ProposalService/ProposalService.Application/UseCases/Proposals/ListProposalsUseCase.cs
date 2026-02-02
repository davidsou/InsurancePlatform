using Insurance.SharedKernel;
using ProposalService.Application.Ports.Inbound.Proposals;
using ProposalService.Application.Ports.Outbound.Persistence;

namespace ProposalService.Application.UseCases.Proposals;

public sealed class ListProposalsUseCase : IListProposalsUseCase
{
    private readonly IProposalRepository _repo;

    public ListProposalsUseCase(IProposalRepository repo) => _repo = repo;

    public async Task<OperationResult<IReadOnlyList<ProposalDto>>> ExecuteAsync(CancellationToken ct)
    {
        var list = await _repo.ListAsync(ct);
        var result = list
            .Select(p => new ProposalDto(p.Id, p.CustomerName, p.ProductCode, p.CoverageAmount, p.Status.ToString(), p.CreatedAtUtc))
            .ToList()
            .AsReadOnly();

        return OperationResult<IReadOnlyList<ProposalDto>>.Ok(result);
    }
}
