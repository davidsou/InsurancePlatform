using ProposalService.Application.Ports.Outbound.Messaging;
using ProposalService.Infrastructure.Persistence;

namespace ProposalService.Infrastructure.Messaging;

public sealed class EfOutboxWriter : IOutboxWriter
{
    private readonly ProposalDbContext _db;

    public EfOutboxWriter(ProposalDbContext db) => _db = db;

    public Task AddAsync(string type, string payloadJson, DateTime occurredAtUtc, CancellationToken ct)
    {
        _db.OutboxMessages.Add(new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            PayloadJson = payloadJson,
            OccurredAtUtc = occurredAtUtc,
            Attempts = 0,
            PublishedAtUtc = null
        });

        return Task.CompletedTask;
    }
}
