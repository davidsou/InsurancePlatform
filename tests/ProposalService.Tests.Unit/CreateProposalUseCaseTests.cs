using FluentAssertions;
using Moq;
using ProposalService.Application.Ports.Inbound.Proposals;
using ProposalService.Application.Ports.Outbound.Persistence;
using ProposalService.Application.Ports.Outbound.Time;
using ProposalService.Application.UseCases.Proposals;

namespace ProposalService.Tests.Unit;

public class CreateProposalUseCaseTests
{
    [Fact]
    public async Task Should_validate_request()
    {
        var repo = new Mock<IProposalRepository>();
        var uow = new Mock<IUnitOfWork>();
        var clock = new Mock<IClock>();
        clock.SetupGet(x => x.UtcNow).Returns(DateTime.UtcNow);

        var uc = new CreateProposalUseCase(repo.Object, uow.Object, clock.Object);

        var res = await uc.ExecuteAsync(new CreateProposalRequest("", "", 0), CancellationToken.None);

        res.IsSuccess.Should().BeFalse();
        res.Error!.Code.Should().Be("validation");
    }
}
