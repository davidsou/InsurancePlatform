using Microsoft.EntityFrameworkCore;
using UnderwritingService.Application.Ports.Outbound.Inbox;
using UnderwritingService.Infrastructure.Persistence;

namespace UnderwritingService.Infrastructure.Inbox;

public sealed class EfInboxStore : IInboxStore
{
    private readonly UnderwritingDbContext _db;

    public EfInboxStore(UnderwritingDbContext db) => _db = db;

    public Task<bool> ExistsAsync(Guid eventId, CancellationToken ct)
        => _db.InboxMessages.AnyAsync(x => x.EventId == eventId, ct);

    public Task StoreAsync(Guid eventId, string type, DateTime receivedAtUtc, CancellationToken ct)
    {
        _db.InboxMessages.Add(new InboxMessage
        {
            EventId = eventId,
            Type = type,
            ReceivedAtUtc = receivedAtUtc,
            ProcessedAtUtc = null
        });
        return Task.CompletedTask;
    }

    public async Task MarkProcessedAsync(Guid eventId, DateTime processedAtUtc, CancellationToken ct)
    {
        var row = await _db.InboxMessages.FirstOrDefaultAsync(x => x.EventId == eventId, ct);
        if (row is not null)
            row.ProcessedAtUtc = processedAtUtc;
    }
}
