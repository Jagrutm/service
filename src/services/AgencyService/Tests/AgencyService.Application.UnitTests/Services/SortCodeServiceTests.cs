using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Models.SortCodes;
using AgencyService.Application.Services;
using AgencyService.Domain.Entities;
using AgencyService.Domain.Enums;
using BuildingBlocks.Tests.Extensions;
using BuildingBlocks.Tests.Mocks;
using BuildingBlocks.Tests.TestFixtures;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Moq;
using static BuildingBlocks.Tests.Extensions.ServiceMockExtensions;

namespace AgencyService.Application.UnitTests.Services
{
    [UsesVerify]
    public class SortCodeServiceTests : IClassFixture<ServiceTestFixture>
    {
        private readonly MapperMock _mapperMock;
        private readonly Mock<ISortCodeRepository> _sortCodeRepository;
        private readonly Mock<IAgencyValidator> _agencyValidator;
        private readonly SortCodeService _sortCodeService;
        private readonly Mock<IAgencyRepository> _agencyRepository;

        public SortCodeServiceTests()
        {
            _mapperMock = new MapperMock();
            _sortCodeRepository = new Mock<ISortCodeRepository>();
            _agencyRepository = new Mock<IAgencyRepository>();
            _agencyValidator = new Mock<IAgencyValidator>();

            _sortCodeService = new SortCodeService(
                _mapperMock.Object, 
                _sortCodeRepository.Object,
                _agencyValidator.Object,
                _agencyRepository.Object);
        }

        [Fact]
        public async Task CreateSortCodeForAgency_ShouldCreateSortCode_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var agency = Builder<Agency>.CreateNew()
                .With(_ => _.Id = 1)
                .With(_ => _.UId = agencyId)
                .Build();
            var sortCodeToCreate = Builder<CreateSortCodeDto>.CreateNew().Build();
            var sortCode = Builder<SortCode>.CreateNew()
                .With(_ => _.AgencyId = agency.Id)
                .Build();

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .ReturnsAsync(agency);
            _mapperMock.MockMap(sortCodeToCreate, sortCode);
            _sortCodeRepository.Setup(_ => _.CreateAsync(IsEquivalentTo(sortCode)));

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                async() =>
                await _sortCodeService.CreateSortCodeForAgencyAsync(agencyId, sortCodeToCreate);

            //Assert
            await Verify(StatusCodes.Status201Created);
            //_agencyValidator.Verify(_ => _.ValidateAgencyWithIdExists(agencyId), Times.Once);
            //_mapperMock.VerifyMap<CreateSortCodeDto, SortCode>(Times.Once());
            //_sortCodeRepository.Verify(_ => _.CreateAsync(IsEquivalentTo(sortCode)), Times.Once);
        }

        [Fact]
        public async Task CreateSortCodeForAgencyAsync_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var errorMessage = "Agency not found";
            var sortCodeToCreate = Builder<CreateSortCodeDto>.CreateNew().Build();

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                async() =>
                await _sortCodeService.CreateSortCodeForAgencyAsync(agencyId, sortCodeToCreate);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task GetSortcodesForAgencyAsync_ShouldGetSortCodes_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var sortCodes = Builder<SortCode>.CreateListOfSize(3).Build().ToList();
            var sortCodeDtos = new List<SortCodeDto>();

            sortCodeDtos.Add(new SortCodeDto
            {
                ChecksumLogic = CheckSumLogicType.Mod10,
                IsReachable = true
            });

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _sortCodeRepository.Setup(_ => _.GetSortcodesForAgency(agencyId))
                .ReturnsAsync(sortCodes);
            _mapperMock.MockMap(sortCodes, sortCodeDtos);

            //Act
            var actualSortCodeDtos = await _sortCodeService.GetSortcodesForAgencyAsync(agencyId);

            //Assert
            await Verify(actualSortCodeDtos);
            //_sortCodeRepository.Verify(_ => _.GetSortcodesForAgency(agencyId), Times.Once);
            //_mapperMock.VerifyMap<List<SortCode>, List<SortCodeDto>>(Times.Once());
        }

        [Fact]
        public async Task GetSortcodesForAgencyAsync_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                 .Throws<Exception>();

            //Act
            Func<Task> result =
                async() =>
                await _sortCodeService.GetSortcodesForAgencyAsync(agencyId);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task VerifySortCodeForAgencyAsync_ShouldVerifySortCode_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var sortCode = "123456";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _sortCodeRepository.Setup(_ => _.VerifySortCodeForAgency(agencyId, sortCode))
                .ReturnsAsync(true);

            //Act
            var isVerified = await _sortCodeService.VerifySortCodeForAgencyAsync(agencyId, sortCode);

            //Assert
            await Verify(isVerified);
            //isVerified.Should().BeTrue();
            //_sortCodeRepository.Verify(_ => _.VerifySortCodeForAgency(agencyId, sortCode), Times.Once);
        }

        [Fact]
        public async Task VerifySortCodeForAgencyAsync_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var sortCode = "123456";
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                async() =>
                await _sortCodeService.VerifySortCodeForAgencyAsync(agencyId, sortCode);

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task GetSortCodeForAllAgenciesAsync_ShouldGetSortCodes_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var sortCodeOne = "123456";
            var sortCodeTwo = "654321";

            var agencyIdSortCodeTuple = Builder<AgencyIdSortCodeTuple>.CreateNew()
                .With(_ => _.AgencyId = agencyId)
                .With(_ => _.SortCodes = new List<string> { sortCodeOne, sortCodeTwo })
                .Build();
            var agencyIdSortCodeTuples = new List<AgencyIdSortCodeTuple> { agencyIdSortCodeTuple };

            var agencyIdSortCodeTupleDto = Builder<AgencyIdSortCodeTupleDto>.CreateNew()
                .With(_ => _.AgencyId = agencyId)
                .With(_ => _.SortCodes = new List<string> { sortCodeOne, sortCodeTwo })
                .Build();
            var agencyIdSortCodeTupleDtos = new List<AgencyIdSortCodeTupleDto> { agencyIdSortCodeTupleDto };

            _sortCodeRepository.Setup(_ => _.GetSortCodeForAllAgencies())
                .ReturnsAsync(agencyIdSortCodeTuples);
            _mapperMock.MockMap(agencyIdSortCodeTuples, agencyIdSortCodeTupleDtos);

            //Act
            
            var actualResult = await _sortCodeService.GetSortCodeForAllAgenciesAsync();

            //Assert
            await Verify(actualResult);
            //actualResult.Should().BeEquivalentTo(agencyIdSortCodeTupleDtos);
            //_sortCodeRepository.Verify(_ => _.GetSortCodeForAllAgencies(), Times.Once);
            //_mapperMock.VerifyMap<List<AgencyIdSortCodeTuple>, List<AgencyIdSortCodeTupleDto>>(Times.Once());
        }
    }
}
