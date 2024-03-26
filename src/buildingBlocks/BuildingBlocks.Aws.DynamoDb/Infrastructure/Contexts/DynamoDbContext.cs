using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BuildingBlocks.Aws.DynamoDb.Infrastructure.Contexts;

namespace BuildingBlocks.Aws.Infrastructure.Contexts
{
    public class DynamoDbContext : DynamoDBContext, IDynamoDbContext
    {
        public DynamoDbContext(IAmazonDynamoDB client) : base(client)
        {

        }
    }
}
