using Microsoft.Extensions.DependencyInjection;
using BuildingBlocks.Dapper.Mappers;
using SqlKata.Compilers;
using Microsoft.Extensions.Configuration;
using BuildingBlocks.Dapper.Contexts;

namespace BuildingBlocks.Infrastructure
{
    public static class DapperServiceCollectionExtensions
    {
        private const string DEFAULT_CONNECTION_STRING = "DefaultConnection";

        public static IServiceCollection AddDapperContext(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultServiceConnectionString = configuration.GetConnectionString(DEFAULT_CONNECTION_STRING);

            services.AddScoped<IDapperContext, MsSqlDapperContext>((sp) =>
            {
                return new MsSqlDapperContext(defaultServiceConnectionString, new SqlServerCompiler());
            });

            services.AddScoped<IDataModelMapper, DataModelMapper>();

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            return services;
        }
    }
}
