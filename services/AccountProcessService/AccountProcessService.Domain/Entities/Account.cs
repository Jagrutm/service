using Amazon.DynamoDBv2.DataModel;
using BuildingBlocks.Core.Domain.Entities;

namespace AccountProcessService.Domain.Entities
{
    [DynamoDBTable("Accounts")]
    public class Account : BaseAuditEntity<long>
    {
        [DynamoDBHashKey]
        public Guid DocumentId { get; set; }

        [DynamoDBProperty]
        public Guid UId { get; set; }

        [DynamoDBProperty]
        public string AccountNumber { get; set; }

        [DynamoDBProperty]
        public string SortCode { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public string? IBAN { get; set; }

        //[Required(ErrorMessage = "You should fill account type id.")]
        //public EnumAccountType AccountTypeId { get; set; }

        [DynamoDBProperty]
        public Guid? AgencyId { get; set; }

        [DynamoDBProperty]
        public bool IsClose { get; set; } = false;
    }
}
