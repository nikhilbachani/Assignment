using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Providers.GetProviderById;

/// <summary>
/// Handler interface for <see cref="GetProviderByIdRequest"/>
/// </summary>
public interface IGetProviderByIdHandler : IRequestHandler<GetProviderByIdRequest, GetProviderByIdResponse>
{ }

/// <summary>
/// Handler implementation for <see cref="IGetProviderByIdHandler"/>
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="providerRepository"></param>
public class GetProviderByIdHandler(ILogger<GetProviderByIdHandler> logger, IValidator<GetProviderByIdRequest> validator, IProviderRepository providerRepository)
  : BaseRequestHandler<GetProviderByIdRequest, GetProviderByIdResponse>(validator), IGetProviderByIdHandler
{
  private readonly ILogger<GetProviderByIdHandler> _logger = logger;
  private readonly IProviderRepository _providerRepository = providerRepository;
  
  /// <summary>
  /// Handles the retrieval of a provider by ID
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<GetProviderByIdResponse>> HandleRequest(GetProviderByIdRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling GetProviderByIdRequest for ID: {Id}", request.Id);

    var provider = await _providerRepository.GetProviderById(request.Id, cancellationToken);

    if (provider == null)
    {
      _logger.LogWarning("No provider found with ID: {Id}", request.Id);
    }

    return ApiResponse<GetProviderByIdResponse>.Succeed(new GetProviderByIdResponse(provider?.ToDto() ?? null));
  }
}
