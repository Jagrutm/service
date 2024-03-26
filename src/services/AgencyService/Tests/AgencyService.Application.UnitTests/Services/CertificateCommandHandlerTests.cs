using AgencyService.Application.Contracts.Persistence;
using AgencyService.Application.Contracts.Validations;
using AgencyService.Application.Features.Certificate.Commands;
using Moq;
using FizzWare.NBuilder;
using AgencyService.Domain.Entities;
using BuildingBlocks.Tests.Extensions;
using AutoMapper;
using BuildingBlocks.Tests.Mocks;
using BuildingBlocks.Tests.TestFixtures;

namespace AgencyService.Application.UnitTests.Services
{
    [UsesVerify]
    public class CertificateCommandHandlerTests : IClassFixture<ServiceTestFixture>
    {
        private readonly MapperMock _mapperMock;
        private readonly Mock<IAgencyValidator> _agencyValidator;
        private readonly Mock<ICertificateValidator> _certificateValidator;
        private readonly Mock<ICertificateRepository> _certificateRepository;

        private readonly ActivateCertificateCommandHandler _activateCertificateCommandHandler;
        private readonly CreateCertificateCommandHandler _createCertificateCommandHandler;
        private readonly DeleteCertificateCommandHandler _deleteCertificateCommandHandler;

        public CertificateCommandHandlerTests()
        {
            _mapperMock = new MapperMock();
            _agencyValidator = new Mock<IAgencyValidator>();
            _certificateValidator = new Mock<ICertificateValidator>();
            _certificateRepository = new Mock<ICertificateRepository>();

            _activateCertificateCommandHandler = new ActivateCertificateCommandHandler(
                _agencyValidator.Object,
                _certificateValidator.Object,
                _certificateRepository.Object);

            _createCertificateCommandHandler = new CreateCertificateCommandHandler(
                _mapperMock.Object,
                _certificateRepository.Object,
                _agencyValidator.Object);

            _deleteCertificateCommandHandler = new DeleteCertificateCommandHandler(
                _certificateRepository.Object,
                _agencyValidator.Object,
                _certificateValidator.Object);
        }

        [Fact]
        public async Task ActivateCertificateCommandHandler_ShouldActivateTheCertificate_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var certificateId = 1;
            var command = Builder<ActivateCertificateCommand>.CreateNew().Build();
            var certificate = Builder<Certificate>.CreateNew().Build();   

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _certificateRepository.Setup(_ => _.GetByIdAsync(certificateId))
                .ReturnsAsync(certificate);
            _certificateValidator.Setup(_ => _.ValidateCertificateWithIdExists(certificateId));
            _certificateRepository.Setup(_ => _.UpdateAsync(certificate))
                .ReturnsAsync(true);

            //Act
            var result = await _activateCertificateCommandHandler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result);
        }

        [Fact]
        public async Task ActivateCertificateCommandHandler_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var command = Builder<ActivateCertificateCommand>.CreateNew().Build();
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                 async () =>
                 await _activateCertificateCommandHandler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task ActivateCertificateCommandHandler_ShouldThrowException_WhenCertificateNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var certificateId = 1;
            var command = Builder<ActivateCertificateCommand>.CreateNew().Build();
            var certificate = Builder<Certificate>.CreateNew().Build();
            var errorMessage = "Certificate not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _certificateRepository.Setup(_ => _.GetByIdAsync(certificateId))
                .ReturnsAsync(certificate);
            _certificateValidator.Setup(_ => _.ValidateCertificateWithIdExists(certificateId))
                  .Throws<Exception>();            

            //Act
            Func<Task> result =
                 async () =>
                 await _activateCertificateCommandHandler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task CreateCertificateCommandHandler_ShouldCreateCertificate_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var agency = Builder<Agency>.CreateNew()
                .With(_=>_.UId = agencyId)
                .Build();
            var command = Builder<CreateCertificateCommand>.CreateNew()
                .With(_ => _.AgencyId = agencyId)
                .Build();
            var certificate = Builder<Certificate>.CreateNew()
                .With(_=>_.AgencyId = 1)
                .Build();

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .ReturnsAsync(agency);
            _mapperMock.MockMap(command, certificate);
            _certificateRepository.Setup(_ => _.CreateAsync(certificate));

            //Act
            var result = await _createCertificateCommandHandler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result);
        }

        [Fact]
        public async Task CreateCertificateCommandHandler_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var command = Builder<CreateCertificateCommand>.CreateNew()
                .With(_ => _.AgencyId = agencyId)
                .Build();
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                async() =>
                await _createCertificateCommandHandler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task DeleteCertificateCommandHandler_ShouldDeleteTheCertificate_WhenRequestIsValid()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var certificateId = 1;
            var command = Builder<DeleteCertificateCommand>.CreateNew().Build();
            var certificate = Builder<Certificate>.CreateNew().Build();

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _certificateRepository.Setup(_ => _.GetByIdAsync(certificateId))
                .ReturnsAsync(certificate);
            _certificateValidator.Setup(_ => _.ValidateCertificateWithIdExists(certificateId));
            _certificateRepository.Setup(_ => _.DeleteAsync(certificate))
                .ReturnsAsync(true);

            //Act
            var result = await _deleteCertificateCommandHandler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result);
        }

        [Fact]
        public async Task DeleteCertificateCommandHandler_ShouldThrowException_WhenAgencyNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var command = Builder<DeleteCertificateCommand>.CreateNew().Build();
            var errorMessage = "Agency not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId))
                .Throws<Exception>();

            //Act
            Func<Task> result =
                 async () =>
                 await _deleteCertificateCommandHandler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }

        [Fact]
        public async Task DeleteCertificateCommandHandler_ShouldThrowException_WhenCertificateNotExists()
        {
            //Arrange
            var agencyId = Guid.NewGuid();
            var certificateId = 1;
            var command = Builder<DeleteCertificateCommand>.CreateNew().Build();
            var certificate = Builder<Certificate>.CreateNew().Build();
            var errorMessage = "Certificate not found";

            _agencyValidator.Setup(_ => _.ValidateAgencyWithIdExists(agencyId));
            _certificateRepository.Setup(_ => _.GetByIdAsync(certificateId))
                .ReturnsAsync(certificate);
            _certificateValidator.Setup(_ => _.ValidateCertificateWithIdExists(certificateId))
                  .Throws<Exception>();

            //Act
            Func<Task> result =
                 async () =>
                 await _deleteCertificateCommandHandler.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            await Verify(result.ThrowExceptionExactly<Exception>(errorMessage));
        }
    }
}
