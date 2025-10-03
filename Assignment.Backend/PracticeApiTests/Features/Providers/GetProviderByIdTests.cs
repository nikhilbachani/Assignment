using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

using PracticeApi.Features.Providers.GetProviderById;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApiTests.Features.Providers;

[TestFixture]
public class GetProviderByIdTests
{
  private GetProviderByIdHandler _handler;
  private readonly GetProviderByIdValidator _validator = new();
  private readonly Mock<IProviderRepository> _mockRepository = new();
  private readonly Mock<ILogger<GetProviderByIdHandler>> _mockLogger = new();

  [SetUp]
  public void Setup()
  {
    _handler = new GetProviderByIdHandler(_mockLogger.Object, _validator, _mockRepository.Object);
  }

  [Test]
  public async Task GetProviderByIdHandler_ShouldReturnProvider_WhenProviderExists()
  {
    // Arrange
    var request = new GetProviderByIdRequest(1);
    var provider = new Provider { Id = 1, FirstName = "Dr.", LastName = "Smith", Specialty = "Cardiology", NpiId = "1234567890" };

    _mockRepository.Setup(r => r.GetProviderById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(provider);

    // Act
    var result = await _handler.Handle(request, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Body.Should().NotBeNull();
    result.Body.Provider.Should().NotBeNull();
    result.Body.Provider.ProviderId.Should().Be(1);
    result.Body.Provider.ProviderName.Should().BeEquivalentTo(provider.FirstName + " " + provider.LastName);
  }

  [Test]
  public async Task GetProviderByIdHandler_ShouldReturnApiResponseWithNoProvider_WhenProviderDoesNotExist()
  {
    // Arrange
    var request = new GetProviderByIdRequest(1);

    _mockRepository.Setup(repo => repo.GetProviderById(It.IsAny<int>(), CancellationToken.None))
        .ReturnsAsync((Provider?)null);

    // Act
    var result = await _handler.Handle(request, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Body.Should().NotBeNull();
    result.Body.Provider.Should().BeNull();
  }

  [Test]
  public void GetProviderByNpiIdValidator_ShouldHaveValidationError_WhenIdIsZero()
  {
    // Arrange
    var request = new GetProviderByIdRequest(0); // Invalid ID

    // Act
    var result = _validator.Validate(request);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle(e => e.PropertyName == "Id" && e.ErrorMessage.Contains("Id must be greater than 0"));
  }

  [Test]
  public void GetProviderByIdValidator_ShouldNotHaveValidationError_WhenIdIsValid()
  {
    // Arrange
    var request = new GetProviderByIdRequest(1);

    // Act
    var result = _validator.Validate(request);

    // Assert
    result.IsValid.Should().BeTrue();
    result.Errors.Should().BeEmpty();
  }
}
