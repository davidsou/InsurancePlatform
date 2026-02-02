using Microsoft.EntityFrameworkCore;
using UnderwritingService.Domain;

namespace UnderwritingService.Infrastructure.Persistence;

public sealed class UnderwritingDbContext : DbContext
{
    public DbSet<Underwriting> Underwritings => Set<Underwriting>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

    public UnderwritingDbContext(DbContextOptions<UnderwritingDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Underwriting>(e =>
        {
            e.ToTable("underwritings");
            e.HasKey(x => x.Id);
            e.Property(x => x.ProposalId).IsRequired();
            e.Property(x => x.ContractedAtUtc).IsRequired();
            e.HasIndex(x => x.ProposalId).IsUnique();
        });

        b.Entity<InboxMessage>(e =>
        {
            e.ToTable("inbox_messages");
            e.HasKey(x => x.EventId);
            e.Property(x => x.Type).IsRequired().HasMaxLength(200);
            e.Property(x => x.ReceivedAtUtc).IsRequired();
            e.Property(x => x.ProcessedAtUtc);
        });
    }
}

public sealed class InboxMessage
{
    public Guid EventId { get; set; }
    public string Type { get; set; } = default!;
    public DateTime ReceivedAtUtc { get; set; }
    public DateTime? ProcessedAtUtc { get; set; }
}
