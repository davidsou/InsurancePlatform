using System.Text.Json;
using Insurance.Contracts.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProposalService.Infrastructure.Persistence;

namespace ProposalService.Infrastructure.Messaging;

public sealed class OutboxPublisherWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxPublisherWorker> _logger;

    public OutboxPublisherWorker(IServiceScopeFactory scopeFactory, ILogger<OutboxPublisherWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ProposalDbContext>();
                var publish = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var batch = await db.OutboxMessages
                    .Where(x => x.PublishedAtUtc == null)
                    .OrderBy(x => x.OccurredAtUtc)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                foreach (var msg in batch)
                {
                    try
                    {
                        if (msg.Type == nameof(ProposalApprovedV1))
                        {
                            var evt = JsonSerializer.Deserialize<ProposalApprovedV1>(msg.PayloadJson);
                            if (evt is not null)
                                await publish.Publish(evt, stoppingToken);
                        }

                        msg.PublishedAtUtc = DateTime.UtcNow;
                        msg.LastError = null;
                    }
                    catch (Exception ex)
                    {
                        msg.Attempts++;
                        msg.LastError = ex.Message;
                        _logger.LogError(ex, "Failed publishing outbox message {Id}", msg.Id);
                    }
                }

                if (batch.Count > 0)
                    await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox loop error");
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
    }
}
