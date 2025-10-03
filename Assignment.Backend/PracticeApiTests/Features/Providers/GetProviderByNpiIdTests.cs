using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

using PracticeApi.Features.Providers.GetProviderByNpiId;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApiTests.Features.Providers;

[TestFixture]
public class GetProviderByNpiIdTests
{
  private GetProviderByNpiIdHandler _handler;
  private readonly GetProviderByNpiIdValidator _validator = new();
  private readonly Mock<IProviderRepository> _mockRepository = new();
  private readonly Mock<ILogger<GetProviderByNpiIdHandler>> _mockLogger = new();

  [SetUp]
  public void Setup()
  {
    _handler = new GetProviderByNpiIdHandler(_mockLogger.Object, _validator, _mockRepository.Object);
  }

  [Test]
  public async Task GetProviderByNpiIdHandler_ShouldReturnProvider_WhenProviderExists()
  {
    // Arrange
    var request = new GetProviderByNpiIdRequest("1234567890");
    var provider = new Provider { Id = 1, FirstName = "Dr.", LastName = "Smith", Specialty = "Cardiology", NpiId = "1234567890" };

    _mockRepository.Setup(r => r.GetProviderByNpiId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
  public async Task GetProviderByNpiIdHandler_ShouldReturnApiResponseWithNoProvider_WhenProviderDoesNotExist()
  {
    // Arrange
    var request = new GetProviderByNpiIdRequest("1234567890");

    _mockRepository.Setup(repo => repo.GetProviderByNpiId(It.IsAny<string>(), CancellationToken.None))
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
    var request = new GetProviderByNpiIdRequest("1");

    // Act
    var result = _validator.Validate(request);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle(e => e.PropertyName == "NpiId" && e.ErrorMessage.Contains("NPI ID must be a 10-digit number"));
  }

  [Test]
  public void GetProviderByNpiIdValidator_ShouldNotHaveValidationError_WhenIdIsValid()
  {
    // Arrange
    var request = new GetProviderByNpiIdRequest("1234567890");

    // Act
    var result = _validator.Validate(request);

    // Assert
    result.IsValid.Should().BeTrue();
    result.Errors.Should().BeEmpty();
  }
}
