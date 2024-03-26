namespace AgencyService.Domain.Entities
{
    public class AgencyIdSortCodeTuple
    {
        public Guid AgencyId { get; set; }

        public List<string> SortCodes { get; set; }
    }
}
