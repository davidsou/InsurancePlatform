namespace ProposalService.Application.Ports.Inbound.Proposals;

public sealed record CreateProposalRequest(string CustomerName, string ProductCode, decimal CoverageAmount);
public sealed record CreateProposalResponse(Guid ProposalId, string Status);

public sealed record ProposalDto(
    Guid Id,
    string CustomerName,
    string ProductCode,
    decimal CoverageAmount,
    string Status,
    DateTime CreatedAtUtc
);

public sealed record ChangeProposalStatusRequest(string Status); // "Approved" | "Rejected"
