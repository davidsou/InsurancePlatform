using Microsoft.EntityFrameworkCore;
using ProposalService.Domain;

namespace ProposalService.Infrastructure.Persistence;

public sealed class ProposalDbContext : DbContext
{
    public DbSet<Proposal> Proposals => Set<Proposal>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public ProposalDbContext(DbContextOptions<ProposalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Proposal>(e =>
        {
            e.ToTable("proposals");
            e.HasKey(x => x.Id);
            e.Property(x => x.CustomerName).IsRequired().HasMaxLength(200);
            e.Property(x => x.ProductCode).IsRequired().HasMaxLength(50);
            e.Property(x => x.CoverageAmount).IsRequired();
            e.Property(x => x.Status).IsRequired();
            e.Property(x => x.CreatedAtUtc).IsRequired();
        });

        b.Entity<OutboxMessage>(e =>
        {
            e.ToTable("outbox_messages");
            e.HasKey(x => x.Id);
            e.Property(x => x.OccurredAtUtc).IsRequired();
            e.Property(x => x.Type).IsRequired().HasMaxLength(200);
            e.Property(x => x.PayloadJson).IsRequired();
            e.Property(x => x.PublishedAtUtc);
            e.Property(x => x.Attempts).IsRequired();
            e.Property(x => x.LastError);
            e.HasIndex(x => x.PublishedAtUtc);
        });
    }
}

public sealed class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; set; }
    public string Type { get; set; } = default!;
    public string PayloadJson { get; set; } = default!;
    public DateTime? PublishedAtUtc { get; set; }
    public int Attempts { get; set; }
    public string? LastError { get; set; }
}
