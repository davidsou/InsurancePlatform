using Insurance.Contracts.Events;
using UnderwritingService.Application.Ports.Inbound.Events;
using UnderwritingService.Application.Ports.Outbound.Inbox;
using UnderwritingService.Application.Ports.Outbound.Persistence;
using UnderwritingService.Application.Ports.Outbound.Time;
using UnderwritingService.Domain;

namespace UnderwritingService.Application.UseCases.Events;

public sealed class HandleProposalApproved : IHandleProposalApproved
{
    private readonly IInboxStore _inbox;
    private readonly IUnderwritingRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IClock _clock;

    public HandleProposalApproved(IInboxStore inbox, IUnderwritingRepository repo, IUnitOfWork uow, IClock clock)
    {
        _inbox = inbox;
        _repo = repo;
        _uow = uow;
        _clock = clock;
    }

    public async Task HandleAsync(ProposalApprovedV1 evt, CancellationToken ct)
    {
        if (await _inbox.ExistsAsync(evt.EventId, ct))
            return;

        await _inbox.StoreAsync(evt.EventId, nameof(ProposalApprovedV1), _clock.UtcNow, ct);

        if (!await _repo.ExistsByProposalIdAsync(evt.ProposalId, ct))
        {
            var underwriting = Underwriting.Create(evt.ProposalId, _clock.UtcNow);
            await _repo.AddAsync(underwriting, ct);
        }

        await _inbox.MarkProcessedAsync(evt.EventId, _clock.UtcNow, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
