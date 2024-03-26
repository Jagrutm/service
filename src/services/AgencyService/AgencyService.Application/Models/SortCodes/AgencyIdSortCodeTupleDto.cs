using AgencyService.Domain.Entities;

namespace AgencyService.Application.Models.SortCodes
{
    public class AgencyIdSortCodeTupleDto
    {
        public Guid AgencyId { get; set; }

        public List<string> SortCodes { get; set; }
    }
}
