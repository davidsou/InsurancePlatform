namespace UnderwritingService.Application.Ports.Outbound.Inbox;

public interface IInboxStore
{
    Task<bool> ExistsAsync(Guid eventId, CancellationToken ct);
    Task StoreAsync(Guid eventId, string type, DateTime receivedAtUtc, CancellationToken ct);
    Task MarkProcessedAsync(Guid eventId, DateTime processedAtUtc, CancellationToken ct);
}
