
using MediatR;

namespace AgencyService.Application.Features.Agency.Commands.DeleteAgency;

public class DeleteAgencyCommand : IRequest
{
    public Guid AgencyId { get; set; }

}
