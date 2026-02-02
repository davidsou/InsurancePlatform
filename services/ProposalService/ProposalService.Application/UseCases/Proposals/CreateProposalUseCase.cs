using Insurance.SharedKernel;
using ProposalService.Application.Ports.Inbound.Proposals;
using ProposalService.Application.Ports.Outbound.Persistence;
using ProposalService.Application.Ports.Outbound.Time;
using ProposalService.Domain;

namespace ProposalService.Application.UseCases.Proposals;

public sealed class CreateProposalUseCase : ICreateProposalUseCase
{
    private readonly IProposalRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IClock _clock;

    public CreateProposalUseCase(IProposalRepository repo, IUnitOfWork uow, IClock clock)
    {
        _repo = repo;
        _uow = uow;
        _clock = clock;
    }

    public async Task<OperationResult<CreateProposalResponse>> ExecuteAsync(CreateProposalRequest request, CancellationToken ct)
    {
        var v = Validate(request);
        if (!v.IsSuccess)
            return OperationResult<CreateProposalResponse>.Fail(v.Error!.Code, v.Error!.Message);

        var proposal = new Proposal(request.CustomerName, request.ProductCode, request.CoverageAmount, _clock.UtcNow);

        await _repo.AddAsync(proposal, ct);
        await _uow.SaveChangesAsync(ct);

        return OperationResult<CreateProposalResponse>.Ok(new CreateProposalResponse(proposal.Id, proposal.Status.ToString()));
    }

    private static OperationResult Validate(CreateProposalRequest r)
    {
        if (string.IsNullOrWhiteSpace(r.CustomerName))
            return Errors.Validation("CustomerName is required.");

        if (string.IsNullOrWhiteSpace(r.ProductCode))
            return Errors.Validation("ProductCode is required.");

        if (r.CoverageAmount <= 0)
            return Errors.Validation("CoverageAmount must be > 0.");

        return OperationResult.Ok();
    }
}
