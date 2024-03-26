
using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgencyService.Application.Features.Agency.Commands.DeleteAgency;

public class DeleteAgencyCommandHandler : IRequestHandler<DeleteAgencyCommand>
{
    private readonly IAgencyRepository _agencyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteAgencyCommandHandler> _logger;

    public DeleteAgencyCommandHandler(
        IAgencyRepository agencyRepository, 
        IMapper mapper, 
        ILogger<DeleteAgencyCommandHandler> logger)
    {
        _agencyRepository = agencyRepository ?? throw new ArgumentNullException(nameof(agencyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(
        DeleteAgencyCommand request, 
        CancellationToken cancellationToken)
    {
        var agencyToDelete = await _agencyRepository.GetAgencyByUIdAsync(request.AgencyId);
        if (agencyToDelete == null)
        {
            throw new NotFoundException(nameof(Agency), request.AgencyId);
        }

        await _agencyRepository.DeleteAsync(agencyToDelete);

        _logger.LogInformation($"Agency {agencyToDelete.Id} is successfully deleted.");

        return Unit.Value;
    }
}
