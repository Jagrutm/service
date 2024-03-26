using AgencyService.Domain.Enums;

namespace AgencyService.Application.Models.Maintenances
{
    public abstract class MaintenanceBaseDto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ResponseCode { get; set; }
    }
}
