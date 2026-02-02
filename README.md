# Insurance Platform — Hexagonal & Event-Driven (.NET 8)

> Technical challenge implementation focused on Hexagonal Architecture,
> Domain-Driven Design, and Event-Driven communication using .NET 8.

This repository contains a **simplified insurance platform** designed to demonstrate
**architectural and design skills**, not feature completeness.

The solution applies **Hexagonal Architecture (Ports & Adapters)** combined with
**DDD principles** and **asynchronous communication** via messaging.

---

## 🎯 Purpose

The main goal of this project is to showcase:

- Clean separation of concerns
- Explicit dependency boundaries
- Event-driven workflows
- Reliability patterns (Outbox / Inbox)
- Testable business logic, isolated from frameworks

Authentication and authorization were intentionally excluded to keep the focus on
architecture and domain modeling.

---

## 🧩 Services Overview

### 📝 ProposalService

Responsible for managing insurance proposals.

**Responsibilities:**
- Create and list proposals
- Change proposal status (`InReview`, `Approved`, `Rejected`)
- Persist domain changes using the **Outbox Pattern**
- Publish domain events asynchronously (`ProposalApprovedV1`)
- Expose a REST API

---

### 🏦 UnderwritingService

Responsible for contracting approved proposals.

**Responsibilities:**
- Consume `ProposalApprovedV1` events via RabbitMQ
- Guarantee idempotent processing using the **Inbox Pattern**
- Create underwriting records only for approved proposals
- Expose REST endpoints for querying underwritings
- Provide a manual trigger endpoint aligned with event-driven constraints

---

## 🏗 Architecture

### Hexagonal Architecture (Ports & Adapters)

- **Domain**: business rules and aggregates
- **Application**: use cases and explicit ports
- **Infrastructure**: EF Core, MassTransit, RabbitMQ, PostgreSQL
- **API**: Controllers acting as inbound adapters

Frameworks and external dependencies are isolated at the edges.

### Event-Driven Communication

- Services do not communicate directly
- Integration happens through domain events
- RabbitMQ is used as the message broker
- MassTransit abstracts messaging infrastructure

### Reliability Patterns

- **Outbox Pattern** in ProposalService to ensure event publishing consistency
- **Inbox Pattern** in UnderwritingService to ensure idempotent message consumption
- Retry and backoff policies configured at the messaging layer

---

## 🗂 Solution Structure
InsurancePlatform.sln
├── shared
│ ├── Insurance.SharedKernel
│ └── Insurance.Contracts
│
├── services
│ ├── ProposalService
│ │ ├── Domain
│ │ ├── Application
│ │ ├── Infrastructure
│ │ ├── CompositionRoot
│ │ └── Api
│ │
│ └── UnderwritingService
│ ├── Domain
│ ├── Application
│ ├── Infrastructure
│ ├── CompositionRoot
│ └── Api
│
└── tests
├── ProposalService.Tests.Unit
└── UnderwritingService.Tests.Unit


---

## 🧪 Testing Strategy

- Unit tests focused on **Domain and Application layers**
- Business rules tested without framework dependencies
- Mocks applied only at port boundaries
- Messaging and infrastructure concerns intentionally excluded from unit tests

---

## 🐳 Running the Project

### Prerequisites
- Docker
- Docker Compose

### Run
```bash
docker compose up --build

Endpoints

ProposalService Swagger
http://localhost:5001/swagger

UnderwritingService Swagger
http://localhost:5002/swagger

RabbitMQ Management UI
http://localhost:15672

User: guest | Password: guest

⚙ Technology Stack

.NET 8

ASP.NET Core (Controllers)

Entity Framework Core

PostgreSQL

MassTransit

RabbitMQ

Docker & Docker Compose

xUnit, Moq, FluentAssertions

🚧 Notes & Trade-offs

Database migrations are not included and can be added per service

Each service owns its own database schema

Mono-repo structure is used for convenience only

Focus is on architecture and design decisions

📌 Final Notes

This project was built as a technical challenge to demonstrate architectural thinking,
clean code practices, and event-driven design.

It intentionally prioritizes:

Decoupling

Explicit boundaries

Testability

Long-term maintainability
