namespace UnderwritingService.Application.Ports.Outbound.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}
