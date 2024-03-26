using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Aws.DynamoDb.Infrastructure.Extensions
{
    public static class ConfigureDynamoDbExtension
    {
        public static IServiceCollection AddDynamoDbServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var dynamoDbConfig = configuration.GetSection("DynamoDb");
            if (dynamoDbConfig == null)
            {
                throw new ArgumentNullException("AWS DynamoDB config not set");
            }

            bool runLocalDynamoDb = Convert.ToBoolean(dynamoDbConfig.GetSection("LocalMode").Value);

            if (runLocalDynamoDb)
            {
                if (string.IsNullOrEmpty(dynamoDbConfig.GetSection("LocalServiceUrl").Value))
                {
                    throw new ArgumentNullException("AWS DynamoDB URL not set");
                }
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = dynamoDbConfig.GetSection("LocalServiceUrl").Value };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddTransient<IDynamoDBContext, DynamoDBContext>();

            return services;
        }
    }
}
