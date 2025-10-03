using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Visits.GetVisitsByDate;

/// <summary>
/// Handler interface for <see cref="GetVisitsByDateRequest"/>
/// </summary>
public interface IGetVisitsByDateHandler : IRequestHandler<GetVisitsByDateRequest, GetVisitsByDateResponse>
{ }

/// <summary>
/// Handler implementation for <see cref="IGetVisitsByDateHandler"/>
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="visitRepository"></param>
public class GetVisitsByDateHandler(ILogger<GetVisitsByDateHandler> logger, IValidator<GetVisitsByDateRequest> validator, IVisitRepository visitRepository)
  : BaseRequestHandler<GetVisitsByDateRequest, GetVisitsByDateResponse>(validator), IGetVisitsByDateHandler
{
  private readonly ILogger<GetVisitsByDateHandler> _logger = logger;
  private readonly IVisitRepository _visitRepository = visitRepository;

  /// <summary>
  /// Handles the request to get visits by date and optional provider ID
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<GetVisitsByDateResponse>> HandleRequest(GetVisitsByDateRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling GetVisitsByDateRequest for date: {VisitDate} and ProviderId: {ProviderId}", request.VisitDate, request.ProviderId ?? 0);

    var visits = await _visitRepository.GetVisitsByDate(request.VisitDate, request.ProviderId, request.IncludeInvoice, cancellationToken);
    
    return ApiResponse<GetVisitsByDateResponse>.Succeed(new GetVisitsByDateResponse(visits.Select(v => v.ToDto()).ToList()));
  }
}
