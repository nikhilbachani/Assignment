using PracticeApi.Shared;
using PracticeApi.Infrastructure.Data;

namespace PracticeApi.Features.Providers.GetProviders;

/// <summary>
/// Handler interface for fetching all providers
/// </summary>
public interface IGetProvidersHandler : IRequestHandler<GetProvidersRequest, GetProvidersResponse>
{ }

/// <summary>
/// Handler implementation for fetching all providers
/// </summary>
/// <param name="providerRepository"></param>
/// <param name="logger"></param>
public class GetProvidersHandler(IProviderRepository providerRepository, ILogger<GetProvidersHandler> logger)
  : BaseRequestHandler<GetProvidersRequest, GetProvidersResponse>(), IGetProvidersHandler
{
  private readonly IProviderRepository _providerRepository = providerRepository;
  private readonly ILogger<GetProvidersHandler> _logger = logger;

  /// <summary>
  /// Handles the retrieval of all providers
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<GetProvidersResponse>> HandleRequest(GetProvidersRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling GetProviders request.");

    var providers = await _providerRepository.GetAllProviders(cancellationToken);
    if (providers.Count == 0)
    {
      _logger.LogWarning("No registered providers found.");
    }

    return ApiResponse<GetProvidersResponse>.Succeed(new GetProvidersResponse(providers.Select(p => p.ToDto()).ToList()));
  }
}
