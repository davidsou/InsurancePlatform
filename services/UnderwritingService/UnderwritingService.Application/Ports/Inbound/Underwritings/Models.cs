namespace UnderwritingService.Application.Ports.Inbound.Underwritings;

public sealed record CreateUnderwritingManualRequest(Guid ProposalId);
public sealed record UnderwritingDto(Guid Id, Guid ProposalId, DateTime ContractedAtUtc);
