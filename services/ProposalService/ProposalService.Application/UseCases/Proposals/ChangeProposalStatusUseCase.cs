using System.Text.Json;
using Insurance.Contracts.Events;
using Insurance.SharedKernel;
using ProposalService.Application.Ports.Inbound.Proposals;
using ProposalService.Application.Ports.Outbound.Messaging;
using ProposalService.Application.Ports.Outbound.Persistence;
using ProposalService.Application.Ports.Outbound.Time;

namespace ProposalService.Application.UseCases.Proposals;

public sealed class ChangeProposalStatusUseCase : IChangeProposalStatusUseCase
{
    private readonly IProposalRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IClock _clock;
    private readonly IOutboxWriter _outbox;

    public ChangeProposalStatusUseCase(IProposalRepository repo, IUnitOfWork uow, IClock clock, IOutboxWriter outbox)
    {
        _repo = repo;
        _uow = uow;
        _clock = clock;
        _outbox = outbox;
    }

    public async Task<OperationResult> ExecuteAsync(Guid proposalId, ChangeProposalStatusRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Status))
            return Errors.Validation("Status is required.");

        var proposal = await _repo.GetByIdAsync(proposalId, ct);
        if (proposal is null)
            return Errors.NotFound("Proposal", proposalId.ToString());

        try
        {
            var status = request.Status.Trim().ToLowerInvariant();

            if (status is "approved")
            {
                proposal.Approve();

                var evt = new ProposalApprovedV1(
                    EventId: Guid.NewGuid(),
                    OccurredAtUtc: _clock.UtcNow,
                    ProposalId: proposal.Id
                );

                var payload = JsonSerializer.Serialize(evt);
                await _outbox.AddAsync(nameof(ProposalApprovedV1), payload, evt.OccurredAtUtc, ct);
            }
            else if (status is "rejected")
            {
                proposal.Reject();
            }
            else
            {
                return Errors.Validation("Status must be 'Approved' or 'Rejected'.");
            }

            await _uow.SaveChangesAsync(ct);
            return OperationResult.Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Errors.BusinessRule(ex.Message);
        }
    }
}
