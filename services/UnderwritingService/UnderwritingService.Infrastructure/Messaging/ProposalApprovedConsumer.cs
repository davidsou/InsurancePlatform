using Insurance.Contracts.Events;
using MassTransit;
using UnderwritingService.Application.Ports.Inbound.Events;

namespace UnderwritingService.Infrastructure.Messaging;

public sealed class ProposalApprovedConsumer : IConsumer<ProposalApprovedV1>
{
    private readonly IHandleProposalApproved _handler;

    public ProposalApprovedConsumer(IHandleProposalApproved handler) => _handler = handler;

    public Task Consume(ConsumeContext<ProposalApprovedV1> context)
        => _handler.HandleAsync(context.Message, context.CancellationToken);
}
