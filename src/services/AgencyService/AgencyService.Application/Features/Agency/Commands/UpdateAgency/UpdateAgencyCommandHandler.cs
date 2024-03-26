using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Features.Agency.Commands.UpdateAgency;

public class UpdateAgencyCommandHandler : IRequestHandler<UpdateAgencyCommand>
{
    private readonly ILogger<UpdateAgencyCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAgencyRepository _agencyRepository;
    private readonly IAgencyValidator _agencyValidator;

    public UpdateAgencyCommandHandler(
        ILogger<UpdateAgencyCommandHandler> logger,
        IMapper mapper,
        IAgencyRepository agencyRepository,
        IAgencyValidator agencyValidator)
    {
        _logger = logger;
        _mapper = mapper;
        _agencyRepository = agencyRepository;
        _agencyValidator = agencyValidator;
    }

    public async Task<Unit> Handle(
        UpdateAgencyCommand request, 
        CancellationToken cancellationToken)
    {
        var existingAgency = await _agencyRepository.GetAgencyByUIdAsync(request.AgencyId);
        _agencyValidator.ValidateAgencyIsNotNull(existingAgency);

        var agencyToUpdate  = _mapper.Map<Domain.Entities.Agency>(request);
        agencyToUpdate.Id = existingAgency.Id;
        await _agencyRepository.UpdateAsync(agencyToUpdate);
        _logger.LogInformation($"Updated agency with id: {agencyToUpdate.Id}");

        return Unit.Value;
    }
}
