using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.Domain.Extensions
{
    public static class ConfigureAwsExtension
    {
        public static IServiceCollection AddAwsServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Get the AWS profile information from configuration providers
            var awsOptions = configuration.GetAWSOptions();
            // Configure AWS service clients to use these credentials
            services.AddDefaultAWSOptions(awsOptions);

            //var dynamoDbConfig = configuration.GetSection("DynamoDb");
            //if (dynamoDbConfig == null)
            //{
            //    throw new ArgumentNullException("AWS DynamoDB config not set");
            //}

            //bool runLocalDynamoDb = Convert.ToBoolean(dynamoDbConfig.GetSection("LocalMode").Value);

            //if (runLocalDynamoDb)
            //{
            //    if (string.IsNullOrEmpty(dynamoDbConfig.GetSection("LocalServiceUrl").Value))
            //    {
            //        throw new ArgumentNullException("AWS DynamoDB URL not set");
            //    }
            //    services.AddSingleton<IAmazonDynamoDB>(sp =>
            //    {
            //        var clientConfig = new AmazonDynamoDBConfig { ServiceURL = dynamoDbConfig.GetSection("LocalServiceUrl").Value };
            //        return new AmazonDynamoDBClient(clientConfig);
            //    });
            //}
            //else
            //{
            //    services.AddAWSService<IAmazonDynamoDB>();
            //}

            //services.AddTransient<IDynamoDBContext, DynamoDBContext>();

            return services;
        }
    }
}
