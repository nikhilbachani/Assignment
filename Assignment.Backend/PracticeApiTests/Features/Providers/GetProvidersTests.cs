using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

using PracticeApi.Features.Providers.GetProviders;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApiTests.Features.Providers;

[TestFixture]
public class GetProvidersTests
{
  private GetProvidersHandler _handler;
  private readonly Mock<IProviderRepository> _mockRepository = new();
  private readonly Mock<ILogger<GetProvidersHandler>> _mockLogger = new();

  [SetUp]
  public void Setup()
  {
    _handler = new GetProvidersHandler(_mockRepository.Object, _mockLogger.Object);
  }

  [Test]
  public async Task GetProvidersHandler_ShouldReturnProviders_WhenProvidersExist()
  {
    // Arrange
    var request = new GetProvidersRequest();
    var providers = new List<Provider>
    {
      new() { Id = 1, FirstName = "Dr.", LastName = "Smith", Specialty = "Cardiology", NpiId = "1234567890" },
      new() { Id = 2, FirstName = "Dr.", LastName = "Jones", Specialty = "Dermatology", NpiId = "0987654321" }
    };

    _mockRepository.Setup(r => r.GetAllProviders(It.IsAny<CancellationToken>()))
      .ReturnsAsync(providers);

    // Act
    var result = await _handler.Handle(request, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Body.Should().NotBeNull();
    result.Body.Providers.Should().HaveCount(2);
  }

  [Test]
  public async Task GetProviderByIdHandler_ShouldReturnApiResponseWithNoProvider_WhenProviderDoesNotExist()
  {
    // Arrange
    var request = new GetProvidersRequest();

    _mockRepository.Setup(repo => repo.GetAllProviders(CancellationToken.None))
        .ReturnsAsync([]);

    // Act
    var result = await _handler.Handle(request, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.Body.Should().NotBeNull();
    result.Body.Providers.Should().BeEmpty();
  }
}
