using Insurance.Contracts.Events;

namespace UnderwritingService.Application.Ports.Inbound.Events;

public interface IHandleProposalApproved
{
    Task HandleAsync(ProposalApprovedV1 evt, CancellationToken ct);
}
