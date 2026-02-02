namespace ProposalService.Domain;

public sealed class Proposal
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string CustomerName { get; private set; } = default!;
    public string ProductCode { get; private set; } = default!;
    public decimal CoverageAmount { get; private set; }
    public ProposalStatus Status { get; private set; } = ProposalStatus.InReview;
    public DateTime CreatedAtUtc { get; private set; }

    private Proposal() { } // EF

    public Proposal(string customerName, string productCode, decimal coverageAmount, DateTime createdAtUtc)
    {
        SetCustomerName(customerName);
        SetProductCode(productCode);
        SetCoverageAmount(coverageAmount);
        CreatedAtUtc = createdAtUtc;
        Status = ProposalStatus.InReview;
    }

    public void Approve()
    {
        if (Status != ProposalStatus.InReview)
            throw new InvalidOperationException("Only proposals in review can be approved.");
        Status = ProposalStatus.Approved;
    }

    public void Reject()
    {
        if (Status != ProposalStatus.InReview)
            throw new InvalidOperationException("Only proposals in review can be rejected.");
        Status = ProposalStatus.Rejected;
    }

    private void SetCustomerName(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("CustomerName is required.");
        CustomerName = value.Trim();
    }

    private void SetProductCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("ProductCode is required.");
        ProductCode = value.Trim().ToUpperInvariant();
    }

    private void SetCoverageAmount(decimal value)
    {
        if (value <= 0) throw new ArgumentException("CoverageAmount must be > 0.");
        CoverageAmount = value;
    }
}
