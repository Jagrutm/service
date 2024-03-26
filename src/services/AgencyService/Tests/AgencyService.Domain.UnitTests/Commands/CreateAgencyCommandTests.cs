using AgencyService.Application.Features.Agency.Commands.CreateAgency;
using AgencyService.Application.UnitTests.Services;
using BuildingBlocks.Tests.TestFixtures;
using FluentValidation.TestHelper;

namespace AgencyService.Domain.UnitTests.Commands
{
    public class CreateAgencyCommandTests : IClassFixture<ServiceTestFixture>
    {
        private readonly CreateAgencyCommandValidator _validator;

        public CreateAgencyCommandTests()
        {
            _validator = new CreateAgencyCommandValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("MoreThanSixChars")]

        public void AgencyCode_ShouldNotBeNullOrEmply_AndHasMaxLegnthUptoSix(string agencyCode)
        {
            //Arrange
            var request = new CreateAgencyCommand
            {
                AgencyCode = agencyCode
            };

            //Act
            TestValidationResult<CreateAgencyCommand> result =
                _validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(_ => _.AgencyCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AgencyName_ShouldNotBeNullOrEmpty(string agencyName)
        {
            //Arrange
            var request = new CreateAgencyCommand
            {
                AgencyName = agencyName
            };

            //Act
            TestValidationResult<CreateAgencyCommand> result =
                _validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(_ => _.AgencyName);
        }
    }
}
