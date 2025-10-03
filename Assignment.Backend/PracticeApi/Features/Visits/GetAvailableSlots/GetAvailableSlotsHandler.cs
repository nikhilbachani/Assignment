using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Visits.GetAvailableSlots;

/// <summary>
/// Handler for getting available slots
/// </summary>
public interface IGetAvailableSlotsHandler : IRequestHandler<GetAvailableSlotsRequest, GetAvailableSlotsResponse>
{ }

/// <summary>
/// Handler implementation for <see cref="IGetAvailableSlotsHandler"/>
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="visitRepository"></param>
public class GetAvailableSlotsHandler(ILogger<GetAvailableSlotsHandler> logger, IValidator<GetAvailableSlotsRequest> validator, IVisitRepository visitRepository)
  : BaseRequestHandler<GetAvailableSlotsRequest, GetAvailableSlotsResponse>(validator), IGetAvailableSlotsHandler
{
  private readonly ILogger<GetAvailableSlotsHandler> _logger = logger;
  private readonly IVisitRepository _visitRepository = visitRepository;

  /// <summary>
  /// Handles the request to get available slots
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<GetAvailableSlotsResponse>> HandleRequest(GetAvailableSlotsRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling GetAvailableSlotsRequest for ProviderId: {ProviderId} on Date: {VisitDate}", request.ProviderId, request.VisitDate);

    var availableSlots = await _visitRepository.GetAvailableSlots(request.ProviderId, request.VisitDate, cancellationToken);

    return ApiResponse<GetAvailableSlotsResponse>.Succeed(new GetAvailableSlotsResponse(availableSlots));
  }
}
