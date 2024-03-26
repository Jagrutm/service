using AgencyService.Application.Features.Agency.Commands.UpdateAgency;
using BuildingBlocks.Tests.TestFixtures;
using FluentValidation.TestHelper;

namespace AgencyService.Domain.UnitTests.Commands
{
    public class UpdateAgencyCommandTests : IClassFixture<ServiceTestFixture>
    {
        private readonly UpdateAgencyCommandValidator _validator;

        public UpdateAgencyCommandTests()
        {
            _validator = new UpdateAgencyCommandValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("MoreThanSixChars")]

        public void AgencyCode_ShouldNotBeNullOrEmply_AndHasMaxLegnthUptoSix(string agencyCode)
        {
            //Arrange
            var request = new UpdateAgencyCommand
            {
                AgencyCode = agencyCode
            };

            //Act
            TestValidationResult<UpdateAgencyCommand> result =
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
            var request = new UpdateAgencyCommand
            {
                AgencyName = agencyName
            };

            //Act
            TestValidationResult<UpdateAgencyCommand> result =
                _validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(_ => _.AgencyName);
        }
    }
}
