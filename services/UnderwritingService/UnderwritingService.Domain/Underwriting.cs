namespace UnderwritingService.Domain;

public sealed class Underwriting
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProposalId { get; private set; }
    public DateTime ContractedAtUtc { get; private set; }

    private Underwriting() { } // EF

    private Underwriting(Guid proposalId, DateTime nowUtc)
    {
        ProposalId = proposalId;
        ContractedAtUtc = nowUtc;
    }

    public static Underwriting Create(Guid proposalId, DateTime nowUtc)
    {
        if (proposalId == Guid.Empty) throw new ArgumentException("ProposalId is required.");
        return new Underwriting(proposalId, nowUtc);
    }
}
