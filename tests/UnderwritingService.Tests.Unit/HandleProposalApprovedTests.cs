using Insurance.Contracts.Events;
using Moq;
using UnderwritingService.Application.Ports.Outbound.Inbox;
using UnderwritingService.Application.Ports.Outbound.Persistence;
using UnderwritingService.Application.Ports.Outbound.Time;
using UnderwritingService.Application.UseCases.Events;
using Xunit;

namespace UnderwritingService.Tests.Unit;

public class HandleProposalApprovedTests
{
    [Fact]
    public async Task Should_be_idempotent_by_eventId()
    {
        var inbox = new Mock<IInboxStore>();
        var repo = new Mock<IUnderwritingRepository>();
        var uow = new Mock<IUnitOfWork>();
        var clock = new Mock<IClock>();
        clock.SetupGet(x => x.UtcNow).Returns(DateTime.UtcNow);

        inbox.Setup(x => x.ExistsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new HandleProposalApproved(inbox.Object, repo.Object, uow.Object, clock.Object);

        var evt = new ProposalApprovedV1(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid());
        await handler.HandleAsync(evt, CancellationToken.None);

        repo.Verify(x => x.AddAsync(It.IsAny<UnderwritingService.Domain.Underwriting>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
