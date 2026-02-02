namespace ProposalService.Application.Ports.Outbound.Messaging;

public interface IOutboxWriter
{
    Task AddAsync(string type, string payloadJson, DateTime occurredAtUtc, CancellationToken ct);
}
