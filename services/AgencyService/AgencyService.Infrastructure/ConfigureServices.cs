using AgencyService.Application.Contracts.Persistence;
using AgencyService.Infrastructure.Persistence;
using AgencyService.Infrastructure.Repositories;
using BuildingBlocks.Infrastructure;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgencyService.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<AgencyDbContext>(options =>
                options.UseInMemoryDatabase("AgencyDB"));
        }
        else
        {
            services.AddDbContext<AgencyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(AgencyDbContext).Assembly.FullName)));

            services.AddDapperContext(configuration);
            services.AddHangfire(_ => _.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();

            //services.AddScoped<IDapperContext, MsSqlDapperContext>((sp) =>
            //{
            //    return new MsSqlDapperContext(defaultServiceConnectionString, new SqlCompiler());
            //});
        }

        //services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IAgencyDbContext>(provider => provider.GetRequiredService<AgencyDbContext>());
        
        services.AddScoped<IAgencyRepository, AgencyRepository>();
        services.AddScoped<ICertificateRepository, CertificateRepository>();
        services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
        services.AddScoped<ISortCodeRepository, SortCodeRepository>();        
        services.AddScoped<IWebhookRepository, WebhookRepository>();

        //services.AddScoped<AgencyDbContextInitialiser>();

        //services
        //    .AddDefaultIdentity<ApplicationUser>()
        //    .AddRoles<IdentityRole>()
        //    .AddEntityFrameworkStores<AgencyDbContext>();

        //services.AddIdentityServer()
        //    .AddApiAuthorization<ApplicationUser, AgencyDbContext>();

        //services.AddTransient<IDateTime, DateTimeService>();
        //services.AddTransient<IIdentityService, IdentityService>();
        //services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        //services.AddAuthentication()
        //    .AddIdentityServerJwt();

        //services.AddAuthorization(options =>
        //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}
