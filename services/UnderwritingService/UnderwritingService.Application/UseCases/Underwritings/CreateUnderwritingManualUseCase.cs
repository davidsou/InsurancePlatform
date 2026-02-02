using Insurance.SharedKernel;
using UnderwritingService.Application.Ports.Inbound.Underwritings;
using UnderwritingService.Application.Ports.Outbound.Persistence;

namespace UnderwritingService.Application.UseCases.Underwritings;

public sealed class CreateUnderwritingManualUseCase : ICreateUnderwritingManualUseCase
{
    private readonly IUnderwritingRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateUnderwritingManualUseCase(IUnderwritingRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<OperationResult> ExecuteAsync(CreateUnderwritingManualRequest request, CancellationToken ct)
    {
        if (request.ProposalId == Guid.Empty)
            return Errors.Validation("ProposalId is required.");

        // Event-driven system: underwriting is created from ProposalApprovedV1.
        // This endpoint acts like a manual trigger/check.
        if (await _repo.ExistsByProposalIdAsync(request.ProposalId, ct))
            return OperationResult.Ok();

        return Errors.Conflict("Underwriting not created yet. Wait for ProposalApproved event to be processed.");
    }
}
