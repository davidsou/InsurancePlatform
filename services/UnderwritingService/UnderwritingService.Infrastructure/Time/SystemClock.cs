using UnderwritingService.Application.Ports.Outbound.Time;

namespace UnderwritingService.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
