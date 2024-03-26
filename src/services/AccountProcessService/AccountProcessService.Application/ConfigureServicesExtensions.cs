using AccountProcessService.Application.Contracts.Services;
using AccountProcessService.Application.Models;
using AccountProcessService.Application.Validations.ModelValidators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AccountProcessService.Application
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddValidatorsFromAssemblyContaining(typeof(CreateAccountDtoValidator));

            //services.AddScoped<IValidator<CreateAccountsDto>, CreateAccountsDtoValidator>();
            //services.AddScoped<IValidator<UpdateAccountDto>, UpdateAccountDtoValidator>();

            services.AddTransient<IAccountProcessService, Services.AccountProcessService>();

            return services;
        }
    }
}
