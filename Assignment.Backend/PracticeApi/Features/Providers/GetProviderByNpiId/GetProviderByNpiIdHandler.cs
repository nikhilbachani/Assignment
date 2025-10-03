using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Providers.GetProviderByNpiId;

/// <summary>
/// Handler interface for fetching a provider by NPI ID
/// </summary>
public interface IGetProviderByNpiIdHandler : IRequestHandler<GetProviderByNpiIdRequest, GetProviderByNpiIdResponse>
{ }

/// <summary>
/// Handler implementation for fetching a provider by ID
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="providerRepository"></param>
public class GetProviderByNpiIdHandler( ILogger<GetProviderByNpiIdHandler> logger, IValidator<GetProviderByNpiIdRequest> validator, IProviderRepository providerRepository)
  : BaseRequestHandler<GetProviderByNpiIdRequest, GetProviderByNpiIdResponse>(validator), IGetProviderByNpiIdHandler
{
  private readonly ILogger<GetProviderByNpiIdHandler> _logger = logger;
  private readonly IProviderRepository _providerRepository = providerRepository;

  /// <summary>
  /// Handles the retrieval of a provider by NPI ID
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns><see cref="GetProviderByNpiIdResponse"/></returns>
  protected override async Task<ApiResponse<GetProviderByNpiIdResponse>> HandleRequest(GetProviderByNpiIdRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling GetProviderByNpiIdRequest for NPI ID: {Id}", request.NpiId);

    var provider = await _providerRepository.GetProviderByNpiId(request.NpiId, cancellationToken);

    if (provider == null)
    {
      _logger.LogWarning("No provider found with NPI ID: {Id}", request.NpiId);
    }

    return ApiResponse<GetProviderByNpiIdResponse>.Succeed(new GetProviderByNpiIdResponse(provider!.ToDto()));
  }
}