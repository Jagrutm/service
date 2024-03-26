using AgencyService.Application.Contracts.Persistence;
using AgencyService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Validations.BusinessValidators
{
    public class SortCodeValidator
    {
        private readonly ILogger<SortCodeValidator> _logger;
        private readonly ISortCodeRepository _sortCodeRepository;

        public SortCodeValidator(ILogger<SortCodeValidator> logger, ISortCodeRepository sortCodeRepository)
        {
            _logger = logger;
            _sortCodeRepository = sortCodeRepository;
        }

        public async Task ValidateSortCodeWithIdExists(int sortCodeId)
        {
            var sortCode = await _sortCodeRepository.GetByIdAsync(sortCodeId);
            ValidateSortCodeIsNotNull(sortCode);
        }

        public void ValidateSortCodeIsNotNull(SortCode sortCode)
        {
            if (sortCode == null)
            {
                _logger.LogInformation("SortCode not found");
                throw new Exception("SortCode not found");
            }

            _logger.LogDebug("Validated sortCode with id {SortCodeId} exists", sortCode?.Id);
        }
    }
}
