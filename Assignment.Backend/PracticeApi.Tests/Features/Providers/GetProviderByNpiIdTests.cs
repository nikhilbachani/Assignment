using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using PracticeApi.Features.Providers.GetProviderByNpiId;
using PracticeApi.Infrastructure.Data;

namespace PracticeApi.Tests.Features.Providers
{
    public class GetProviderByNpiIdTests
    {
        [Fact]
        public async Task GetProviderByNpiIdHandler_ShouldReturnProvider_WhenProviderExists()
        {
            // Arrange
            var mockRepository = new Mock<IProviderRepository>();
            var handler = new GetProviderByNpiIdHandler(mockRepository.Object);
            var request = new GetProviderByNpiIdRequest { NpiId = "1234567890" };
            var provider = new ProviderDto { Id = 1, Name = "Dr. Smith", NpiId = "1234567890" };

            mockRepository.Setup(repo => repo.GetProviderByNpiIdAsync("1234567890", CancellationToken.None))
                          .ReturnsAsync(provider);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Dr. Smith");
            result.NpiId.Should().Be("1234567890");
        }

        [Fact]
        public async Task GetProviderByNpiIdHandler_ShouldReturnNull_WhenProviderDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IProviderRepository>();
            var handler = new GetProviderByNpiIdHandler(mockRepository.Object);
            var request = new GetProviderByNpiIdRequest { NpiId = "1234567890" };

            mockRepository.Setup(repo => repo.GetProviderByNpiIdAsync("1234567890", CancellationToken.None))
                          .ReturnsAsync((ProviderDto)null);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetProviderByNpiIdValidator_ShouldHaveValidationError_WhenNpiIdIsEmpty()
        {
            // Arrange
            var validator = new GetProviderByNpiIdValidator();
            var request = new GetProviderByNpiIdRequest { NpiId = string.Empty };

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "NpiId" && e.ErrorMessage.Contains("must not be empty"));
        }

        [Fact]
        public void GetProviderByNpiIdValidator_ShouldNotHaveValidationError_WhenNpiIdIsValid()
        {
            // Arrange
            var validator = new GetProviderByNpiIdValidator();
            var request = new GetProviderByNpiIdRequest { NpiId = "1234567890" };

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}