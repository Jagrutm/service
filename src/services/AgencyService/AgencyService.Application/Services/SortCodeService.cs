using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Services;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Models.SortCodes;
using AgencyService.Domain.Entities;
using AutoMapper;

namespace AgencyService.Application.Services
{
    public class SortCodeService : ISortCodeService
    {
        private readonly IMapper _mapper; 
        private readonly ISortCodeRepository _sortCodeRepository;
        private readonly IAgencyValidator _agencyValidator;
        private readonly IAgencyRepository _agencyRepository;

        public SortCodeService(
            IMapper mapper, 
            ISortCodeRepository sortCodeRepository,
            IAgencyValidator agencyValidator,
            IAgencyRepository agencyRepository)
        {
            _mapper = mapper;
            _sortCodeRepository = sortCodeRepository;
            _agencyValidator = agencyValidator;
            _agencyRepository = agencyRepository;
        }

        public async Task CreateSortCodeForAgencyAsync(Guid agencyId, CreateSortCodeDto sortCodeToCreate)
        {
            var agency = await _agencyValidator.ValidateAgencyWithIdExists(agencyId);

            var sortCode = _mapper.Map<SortCode>(sortCodeToCreate);
            sortCode.AgencyId = agency.Id;
            await _sortCodeRepository.CreateAsync(sortCode);
        }

        public async Task<List<SortCodeDto>> GetSortcodesForAgencyAsync(Guid agencyId)
        {
            // using query
            await _agencyValidator.ValidateAgencyWithIdExists(agencyId);
            var sortCodes = await _sortCodeRepository.GetSortcodesForAgency(agencyId);
            return _mapper.Map<List<SortCodeDto>>(sortCodes);

            //using stored procedure
            //return await _sortCodeRepository.GetSortcodesForAgencyUsingSp(agencyId);
        }

        public async Task<bool> VerifySortCodeForAgencyAsync(Guid agencyId, string sortCode)
        {
            await _agencyValidator.ValidateAgencyWithIdExists(agencyId);
            return await _sortCodeRepository.VerifySortCodeForAgency(agencyId, sortCode);
        }

        public async Task<List<AgencyIdSortCodeTupleDto>> GetSortCodeForAllAgenciesAsync()
        {
            var agencyIdSortCodeTuples = await _sortCodeRepository.GetSortCodeForAllAgencies();
            return _mapper.Map<List<AgencyIdSortCodeTupleDto>>(agencyIdSortCodeTuples);
        }
    }
}
