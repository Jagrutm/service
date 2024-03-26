using AgencyService.Application.Features.Agency.Commands.CreateAgency;
using AgencyService.Application.Features.Agency.Commands.DeleteAgency;
using AgencyService.Application.Features.Agency.Commands.UpdateAgency;
using AgencyService.Application.Features.Agency.Queries.GetAgency;
using AgencyService.Application.Features.Agency.Queries.GetAgencyList;
using AgencyService.Application.UnitTests.Services;
using AgencyService.Controllers;
using BuildingBlocks.Tests.Extensions;
using BuildingBlocks.Tests.TestFixtures;
using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static BuildingBlocks.Tests.Extensions.ServiceMockExtensions;

namespace AgencyService.Api.UnitTests.Controllers
{
    public class AgencyControllerTests : IClassFixture<ServiceTestFixture>
    {
        private readonly Mock<IMediator> _mediator;
        private readonly AgencyController _agencyController;

        public AgencyControllerTests()
        {
            _mediator = new Mock<IMediator>();
            _agencyController = new AgencyController(_mediator.Object);
        }

        [Fact]
        public async Task CreateAgency_ShouldCreateAgency_WhenRequestIsValid()
        {
            //Arrange
            var command = Builder<CreateAgencyCommand>.CreateNew().Build();
            var response = Builder<AgencyResponse>.CreateNew().Build();

            _mediator.Setup(_ => _.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            IActionResult createdResult = await _agencyController.CreateAgency(command);

            //Assert
            Assert.NotNull(createdResult);
            CreatedResult objectResult = Assert.IsType<CreatedResult>(createdResult);
            objectResult.Value.ShouldBeEquivalent(response);
        }

        [Fact]
        public async Task UpdateAgency_ShouldUpdateAgency_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var command = Builder<UpdateAgencyCommand>.CreateNew().Build();

            _mediator.Setup(_ => _.Send(command, It.IsAny<CancellationToken>()));

            //Act
            IActionResult noContentResult = await _agencyController.UpdateAgency(agencyId, command);

            //Assert
            Assert.NotNull(noContentResult);
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async Task DeleteAgency_ShouldDeleteAgency_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var command = Builder<DeleteAgencyCommand>.CreateNew()
                .With(_=>_.AgencyId = agencyId)
                .Build();
            var unit = Builder<Unit>.CreateNew().Build();

            _mediator.Setup(_ => _.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(unit);

            //Act
            IActionResult noContentResult = await _agencyController.DeleteAgency(agencyId);

            //Assert
            Assert.NotNull(noContentResult);
            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async Task GetAgencies_ShouldGetAllAgenices_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var response = Builder<AgencyResponse>.CreateNew()
                .With(_=>_.AgencyId = agencyId)
                .Build();
            var responses = new List<AgencyResponse> { response };
            
            _mediator.Setup(_ => _.Send(It.IsAny<GetAgenciesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(responses);

            //Act
            IActionResult okResult = await _agencyController.GetAgencies();

            //Assert
            Assert.NotNull(okResult);
            OkObjectResult objectResult = Assert.IsType<OkObjectResult>(okResult);
            objectResult.Value.ShouldBeEquivalent(responses);
        }

        [Fact]
        public async Task GetAgencyById_ShouldGetAgency_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var query = Builder<GetAgencyQuery>.CreateNew()
                .With(_=>_.AgencyId = agencyId)
                .Build();
            var response = Builder<AgencyResponse>.CreateNew()
               .With(_ => _.AgencyId = agencyId)
               .Build();

            _mediator.Setup(_ => _.Send(IsEquivalentTo(query), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            //Act
            IActionResult okResult = await _agencyController.GetAgencyById(agencyId);

            //Assert
            Assert.NotNull(okResult);
            OkObjectResult objectResult = Assert.IsType<OkObjectResult>(okResult);
            objectResult.Value.ShouldBeEquivalent(response);
        }
    }
}
