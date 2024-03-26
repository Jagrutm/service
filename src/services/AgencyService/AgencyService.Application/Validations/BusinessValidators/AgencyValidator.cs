using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Validations.BusinessValidators
{
    public class AgencyValidator : IAgencyValidator
    {
        private readonly ILogger<AgencyValidator> _logger;
        private readonly IAgencyRepository _agencyRepository;

        public AgencyValidator(
            ILogger<AgencyValidator> logger,
            IAgencyRepository agencyRepository)
        {
            _logger = logger;
            _agencyRepository = agencyRepository;
        }

        public async Task<Agency> ValidateAgencyWithIdExists(Guid agencyId)
        {
            var agency = await _agencyRepository.GetAgencyByUIdAsync(agencyId);
            ValidateAgencyIsNotNull(agency);
            return agency;
        }

        public void ValidateAgencyIsNotNull(Agency agency)
        {
            if (agency == null)
            {
                _logger.LogInformation("Agency not found");
                throw new Exception("Agency not found");
            }

            _logger.LogDebug("Validated agency with id {AgencyId} exists", agency?.Id);
        }
    }
}
