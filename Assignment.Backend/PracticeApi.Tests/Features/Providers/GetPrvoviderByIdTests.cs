namespace PracticeApi.Tests;

[TestFixture]
public class GetProviderByIdTests
{
  private Mock<ILogger<GetProviderByIdHandler>> _loggerMock;
  private Mock<IProviderRepository> _providerRepositoryMock;
  private IValidator<GetProviderByIdRequest> _validator;
  private GetProviderByIdHandler _handler;

  [SetUp]
  public void SetUp()
  {
    _loggerMock = new Mock<ILogger<GetProviderByIdHandler>>();
    _providerRepositoryMock = new Mock<IProviderRepository>();
    _validator = new GetProviderByIdValidator();
    _handler = new GetProviderByIdHandler(_loggerMock.Object, _validator, _providerRepositoryMock.Object);
  }

  [Test]
  public async Task HandleRequest_ShouldReturnProvider_WhenProviderExists()
  {
    // Arrange
    var providerId = 1;
    var provider = new Provider { Id = providerId, FirstName = "John", LastName = "Doe" };
    _providerRepositoryMock.Setup(repo => repo.GetProviderById(providerId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(provider);

    var request = new GetProviderByIdRequest(providerId);

    // Act
    var response = await _handler.Handle(request, CancellationToken.None);

    // Assert
    response.Should().NotBeNull();
    response.Body.Should().NotBeNull();
    response.Body.Provider.Should().NotBeNull();
    response.Body.Provider.ProviderId.Should().Be(providerId);
    response.Body.Provider.ProviderName.Should().Be("John Doe");
  }

  [Test]
  public async Task HandleRequest_ShouldReturnNull_WhenProviderDoesNotExist()
  {
    // Arrange
    var providerId = 1;
    _providerRepositoryMock.Setup(repo => repo.GetProviderById(providerId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(default(Provider));

    var request = new GetProviderByIdRequest(providerId);

    // Act
    var response = await _handler.Handle(request, CancellationToken.None);

    // Assert
    response.Should().NotBeNull();
    response.Body.Should().NotBeNull();
    response.Body.Provider.Should().BeNull();
  }

  [Test]
  public void Validator_ShouldHaveNoValidationError_WhenIdIsGreaterThanZero()
  {
    // Arrange
    var request = new GetProviderByIdRequest(1);

    // Act
    var result = _validator.TestValidate(request);

    // Assert
    result.IsValid.Should().BeTrue();
    result.Errors.Should().BeEmpty();
  }

  [Test]
  public void Validator_ShouldHaveValidationError_WhenIdIsZeroOrNegative()
  {
    // Arrange
    var request = new GetProviderByIdRequest(0);

    // Act
    var result = _validator.TestValidate(request);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Id must be greater than 0");
  }
}