using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

using PracticeApi.Features.Providers.AddProvider;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApiTests.Features.Providers;

[TestFixture]
public class AddProviderTests
{
  private AddProviderHandler _handler;
  private readonly AddProviderValidator _validator = new();
  private readonly Mock<IProviderRepository> _mockRepository = new();
  private readonly Mock<ILogger<AddProviderHandler>> _mockLogger = new();

  [SetUp]
  public void Setup()
  {
    _handler = new AddProviderHandler(_mockLogger.Object, _validator, _mockRepository.Object);
  }

  [Test]
  public async Task AddProviderHandler_ShouldAddProvider_WhenRequestIsValid()
  {
    // Arrange
    var request = new AddProviderRequest("Dr.", "Smith", "Cardiology", "1234567890");
    _mockRepository.Setup(r => r.AddProvider(It.IsAny<Provider>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(1);

    // Act
    var result = await _handler.Handle(request, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Body.Should().NotBeNull();
    result.Body.ProviderId.Should().Be(1);
  }

  [Test]
  public void AddProviderValidator_ShouldHaveValidationErrors_WhenRequestIsInvalid()
  {
    // Arrange
    var request = new AddProviderRequest("", "", "", "");
    
    // Act
    var result = _validator.Validate(request);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle(e => e.PropertyName == "FirstName" && e.ErrorMessage.Contains("First name is required"));
    result.Errors.Should().ContainSingle(e => e.PropertyName == "LastName" && e.ErrorMessage.Contains("Last name is required"));
    result.Errors.Should().ContainSingle(e => e.PropertyName == "Specialty" && e.ErrorMessage.Contains("Specialty is required"));
    result.Errors.Should().ContainSingle(e => e.PropertyName == "NpiId" && e.ErrorMessage.Contains("NPI ID must be a 10-digit number"));
  }

  [Test]
  public void GetProviderByIdValidator_ShouldNotHaveValidationErrors_WhenIdIsValid()
  {
    // Arrange
    var request = new AddProviderRequest("Dr.", "Smith", "Cardiology", "1234567890");

    // Act
    var result = _validator.Validate(request);

    // Assert
    result.IsValid.Should().BeTrue();
    result.Errors.Should().BeEmpty();
  }
}
