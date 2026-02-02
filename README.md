# Insurance Platform (Hexagonal + MassTransit + RabbitMQ + Postgres)

## Services
- **ProposalService**: creates proposals, changes status, writes Outbox, publishes events.
- **UnderwritingService**: consumes `ProposalApprovedV1`, stores Inbox (dedup), creates underwritings.

## Run with Docker
```bash
docker compose up --build
```

- ProposalService Swagger: http://localhost:5001/swagger
- UnderwritingService Swagger: http://localhost:5002/swagger
- RabbitMQ UI: http://localhost:15672 (guest/guest)

## Notes
This skeleton includes EF DbContexts but **does not include migrations yet**.
You can add migrations per service:
- ProposalService: `ProposalDbContext`
- UnderwritingService: `UnderwritingDbContext`
