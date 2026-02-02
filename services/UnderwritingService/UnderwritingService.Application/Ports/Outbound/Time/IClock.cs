namespace UnderwritingService.Application.Ports.Outbound.Time;

public interface IClock
{
    DateTime UtcNow { get; }
}
