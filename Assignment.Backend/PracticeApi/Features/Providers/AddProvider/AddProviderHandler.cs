using FluentValidation;

using PracticeApi.Shared;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Features.Providers.AddProvider;

/// <summary>
/// Handler interface for adding a new provider
/// </summary>
public interface IAddProviderHandler : IRequestHandler<AddProviderRequest, AddProviderResponse>
{}

/// <summary>
/// Handler implementation for adding a new provider
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="repository"></param>
public class AddProviderHandler(ILogger<AddProviderHandler> logger, IValidator<AddProviderRequest> validator, IProviderRepository repository)
  : BaseRequestHandler<AddProviderRequest, AddProviderResponse>(validator), IAddProviderHandler
{
  private readonly ILogger<AddProviderHandler> _logger = logger;
  private readonly IProviderRepository _providerRepository = repository;

  /// <summary>
  /// Handles the addition of a new provider
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns><see cref="AddProviderResponse"/></returns>
  protected override async Task<ApiResponse<AddProviderResponse>> HandleRequest(AddProviderRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling AddProviderRequest for NPI ID: {NpiId}", request.NpiId);

    var newProvider = new Provider
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      Specialty = request.Specialty,
      NpiId = request.NpiId
    };

    var id = await _providerRepository.AddProvider(newProvider, cancellationToken);
    
    return ApiResponse<AddProviderResponse>.Succeed(new AddProviderResponse(id));
  }
}
