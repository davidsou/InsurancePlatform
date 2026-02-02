using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnderwritingService.Application.Ports.Inbound.Events;
using UnderwritingService.Application.Ports.Inbound.Underwritings;
using UnderwritingService.Application.Ports.Outbound.Inbox;
using UnderwritingService.Application.Ports.Outbound.Persistence;
using UnderwritingService.Application.Ports.Outbound.Time;
using UnderwritingService.Application.UseCases.Events;
using UnderwritingService.Application.UseCases.Underwritings;
using UnderwritingService.Infrastructure.Inbox;
using UnderwritingService.Infrastructure.Messaging;
using UnderwritingService.Infrastructure.Persistence;
using UnderwritingService.Infrastructure.Time;

namespace UnderwritingService.CompositionRoot;

public static class DependencyInjection
{
    public static IServiceCollection AddUnderwritingService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UnderwritingDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("UnderwritingDb")));

        // Outbound adapters
        services.AddScoped<IUnderwritingRepository, EfUnderwritingRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IInboxStore, EfInboxStore>();
        services.AddSingleton<IClock, SystemClock>();

        // Inbound ports
        services.AddScoped<IHandleProposalApproved, HandleProposalApproved>();
        services.AddScoped<IListUnderwritingsUseCase, ListUnderwritingsUseCase>();
        services.AddScoped<IGetUnderwritingUseCase, GetUnderwritingUseCase>();
        services.AddScoped<IGetByProposalUseCase, GetByProposalUseCase>();
        services.AddScoped<ICreateUnderwritingManualUseCase, CreateUnderwritingManualUseCase>();

        // MassTransit consumer
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProposalApprovedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["Rabbit:Host"], h =>
                {
                    h.Username(configuration["Rabbit:User"]);
                    h.Password(configuration["Rabbit:Pass"]);
                });

                cfg.ReceiveEndpoint("underwriting.proposal-approved", e =>
                {
                    e.ConfigureConsumer<ProposalApprovedConsumer>(ctx);

                    e.UseMessageRetry(r =>
                        r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(2)));
                });
            });
        });

        return services;
    }
}
