namespace Insurance.Contracts.Events;

public sealed record ProposalApprovedV1(
    Guid EventId,
    DateTime OccurredAtUtc,
    Guid ProposalId
);
