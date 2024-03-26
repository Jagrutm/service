using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Features.Agency.Commands.CreateAgency;
using AgencyService.Application.Features.Agency.Queries.GetAgency;
using AgencyService.Application.Features.Agency.Queries.GetAgencyList;
using AgencyService.Domain.Entities;
using BuildingBlocks.Tests.Mocks;
using BuildingBlocks.Tests.TestFixtures;
using FizzWare.NBuilder;
using Moq;

namespace AgencyService.Application.UnitTests.Services
{
    [UsesVerify]
    public class AgencyCommandHandlerTests : IClassFixture<ServiceTestFixture>
    {
        private readonly MapperMock _mapperMock;
        private readonly Mock<IAgencyValidator> _agencyValidator;
        private readonly Mock<IAgencyRepository> _agencyRepositary;

        private readonly CreateAgencyCommandHandler _createAgencyCommandHandler;
        private readonly GetAgenciesQueryHandler _getAgenciesQueryHandler;

        public AgencyCommandHandlerTests()
        {
            _mapperMock = new MapperMock();
            _agencyValidator = new Mock<IAgencyValidator>();
            _agencyRepositary = new Mock<IAgencyRepository>();

            _createAgencyCommandHandler = new CreateAgencyCommandHandler(
                _agencyRepositary.Object,
                _mapperMock.Object);

            _getAgenciesQueryHandler = new GetAgenciesQueryHandler(
                _agencyRepositary.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAgencyCommandHandler_ShouldCreateAgency_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var request = Builder<CreateAgencyCommand>.CreateNew().Build();
            var agency = Builder<Agency>.CreateNew()
                .With(_=>_.UId = agencyId)
                .Build();
            var response = Builder<AgencyResponse>.CreateNew()
                .With(_=>_.AgencyName = "Contis by Solaris")
                .With(_=>_.AgencyCode = "CON123")
                .With(_=>_.AgencyId = agency.UId)
                .Build();

            _mapperMock.MockMap(request, agency);
            _agencyRepositary.Setup(_ => _.CreateAsync(agency))
                .ReturnsAsync(agency);
            _mapperMock.MockMap(request, response);
                
            //Act
            var result = await _createAgencyCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result);
        }

        [Fact]
        public async Task GetAgenciesQueryHandler_ShouldGetAgencies_WhenRequetIsValid()
        {
            //Arrange
            var agency = Builder<Agency>.CreateNew()
                .With(_=>_.UId = Guid.NewGuid())
                .Build();
            var agencies = new List<Agency> { agency };
            var request = Builder<GetAgenciesQuery>.CreateNew().Build();
            var agencyResponse = Builder<AgencyResponse>.CreateNew()
                .With(_ => _.AgencyId = agency.UId)
                .Build();
            var agencyResponses = new List<AgencyResponse> { agencyResponse };

            _agencyRepositary.Setup(_ => _.GetAllAsync())
                .ReturnsAsync(agencies);            
            _mapperMock.MockMap(agencies, agencyResponses);

            //Act
            var result = await _getAgenciesQueryHandler.Handle(request, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result);
        }
    }
}
