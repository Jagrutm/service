using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Services;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Models.Maintenances;
using AgencyService.Domain.Entities;
using AgencyService.Domain.Enums;
using AutoMapper;

namespace AgencyService.Application.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMapper _mapper;
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IMaintenanceValidator _maintenanceValidator;
        private readonly IAgencyValidator _agencyValidator;

        public MaintenanceService(
            IMapper mapper,
            IMaintenanceRepository maintenanceRepository,
            IMaintenanceValidator maintenanceValidator,
            IAgencyValidator agencyValidator)
        {
            _mapper = mapper;
            _maintenanceRepository = maintenanceRepository;
            _maintenanceValidator = maintenanceValidator;
            _agencyValidator = agencyValidator;
        }

        public async Task CreateMaintenanceForAgencyAsync(Guid agencyId, CreateMaintenanceDto maintenanceDto)
        {
            var agency = await _agencyValidator.ValidateAgencyWithIdExists(agencyId);

            var maintenanceToCreate = _mapper.Map<Maintenance>(maintenanceDto);
            maintenanceToCreate.AgencyId = agency.Id;
            await _maintenanceRepository.CreateAsync(maintenanceToCreate);
        }

        public async Task<List<MaintenanceDto>> GetMaintenancesAsync(Guid agencyId)
        { 
            await _agencyValidator.ValidateAgencyWithIdExists(agencyId);
            var existingMaintenances = await _maintenanceRepository.GetAllAsync();
            return _mapper.Map<List<MaintenanceDto>>(existingMaintenances);
        }

        public async Task<MaintenanceDto> GetMaintenanceDetailsAsync(Guid agencyId, int maintenanceId)
        {
            await _agencyValidator.ValidateAgencyWithIdExists(agencyId);

            var existingMaintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            _maintenanceValidator.ValidateMaintenanceIsNotNull(existingMaintenance);
            return _mapper.Map<MaintenanceDto>(existingMaintenance);
        }

        public async Task UpdateMaintenanceForAgencyAsync(Guid agencyId, int maintenanceId, UpdateMaintenanceDto maintenanceDto)
        {
            await _agencyValidator.ValidateAgencyWithIdExists(agencyId);

            var existingMaintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            _maintenanceValidator.ValidateMaintenanceIsNotNull(existingMaintenance);

            existingMaintenance.FromDate = maintenanceDto.FromDate;
            existingMaintenance.ToDate = maintenanceDto.ToDate;
            existingMaintenance.ResponseCode = Enum.Parse<QualifiedAcceptanceCode>(maintenanceDto.ResponseCode);
            existingMaintenance.LastModifiedOn = DateTime.Now;
            //existingMaintenance.LastModifiedBy = userId or userName; //TODO:

            await _maintenanceRepository.UpdateAsync(existingMaintenance);
        }

        public async Task DeleteMaintenanceForAgencyAsync(Guid agencyId, int maintenanceId)
        {
            await _agencyValidator.ValidateAgencyWithIdExists(agencyId);

            var existingMaintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            _maintenanceValidator.ValidateMaintenanceIsNotNull(existingMaintenance);

            await _maintenanceRepository.DeleteAsync(existingMaintenance);
        }
    }
}
