namespace AccountProcessService.Application.Models
{

    public class CreateAccountsDto
    {
        public List<CreateAccountDto> Accounts { get; set; }
    }

    public class CreateAccountDto
    {
        public string AccountNumber { get; set; }

        public string SortCode { get; set; }

        public string Name { get; set; }

        public string? IBAN { get; set; }

        //[Required(ErrorMessage = "You should fill account type id.")]
        //public EnumAccountType AccountTypeId { get; set; }

        public Guid? AgencyId { get; set; }
    }
}
