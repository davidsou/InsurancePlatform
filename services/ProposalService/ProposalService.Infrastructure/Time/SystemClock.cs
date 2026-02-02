using ProposalService.Application.Ports.Outbound.Time;

namespace ProposalService.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
