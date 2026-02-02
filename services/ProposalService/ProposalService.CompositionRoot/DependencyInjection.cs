using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProposalService.Application.Ports.Inbound.Proposals;
using ProposalService.Application.Ports.Outbound.Messaging;
using ProposalService.Application.Ports.Outbound.Persistence;
using ProposalService.Application.Ports.Outbound.Time;
using ProposalService.Application.UseCases.Proposals;
using ProposalService.Infrastructure.Messaging;
using ProposalService.Infrastructure.Persistence;
using ProposalService.Infrastructure.Time;

namespace ProposalService.CompositionRoot;

public static class DependencyInjection
{
    public static IServiceCollection AddProposalService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProposalDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("ProposalDb")));

        // Outbound adapters
        services.AddScoped<IProposalRepository, EfProposalRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddSingleton<IClock, SystemClock>();
        services.AddScoped<IOutboxWriter, EfOutboxWriter>();

        // Inbound use cases
        services.AddScoped<ICreateProposalUseCase, CreateProposalUseCase>();
        services.AddScoped<IGetProposalUseCase, GetProposalUseCase>();
        services.AddScoped<IListProposalsUseCase, ListProposalsUseCase>();
        services.AddScoped<IChangeProposalStatusUseCase, ChangeProposalStatusUseCase>();

        // MassTransit publisher
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["Rabbit:Host"], h =>
                {
                    h.Username(configuration["Rabbit:User"]);
                    h.Password(configuration["Rabbit:Pass"]);
                });
            });
        });

        services.AddHostedService<OutboxPublisherWorker>();
        return services;
    }
}
