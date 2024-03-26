using AgencyService.Domain.Entities;

namespace AgencyService.Application.Contracts.Validations
{
    public interface IAgencyValidator
    {
        Task<Agency> ValidateAgencyWithIdExists(Guid agencyId);

        void ValidateAgencyIsNotNull(Agency agency);
    }
}
