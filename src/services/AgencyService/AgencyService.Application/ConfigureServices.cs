
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AgencyService.Application.Contracts.Services;
using AgencyService.Application.Services;
using AgencyService.Application.Features.Certificate.Commands;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Validations.BusinessValidators;
using BuildingBlocks.Application.Behaviours;
using FluentValidation;
using AgencyService.Application.Features.Agency.Commands.CreateAgency;

namespace AgencyService.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMeditor();
        services.AddValidator();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services.AddTransient<ISortCodeService, SortCodeService>();
        services.AddTransient<IMaintenanceService, MaintenanceService>();
        services.AddTransient<IWebhookService, WebhookService>();
        services.AddTransient<IJobTestService, JobTestService>();

        return services;
    }

    private static void AddMeditor(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateAgencyCommand).GetTypeInfo().Assembly);
        services.AddMediatR(typeof(CreateCertificateCommand).GetTypeInfo().Assembly);
        services.AddMediatR(typeof(DeleteCertificateCommand).GetTypeInfo().Assembly);
    }

    private static void AddValidator(this IServiceCollection services)
    {
        services.AddTransient<IAgencyValidator, AgencyValidator>();
        services.AddTransient<ICertificateValidator, CertificateValidator>();
        services.AddTransient<IMaintenanceValidator, MaintenanceValidator>();
        services.AddTransient<IWebhookValidator, WebhookValidator>();
    }
}
