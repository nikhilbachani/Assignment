using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Visits.GetVisitById;

/// <summary>
/// Handler interface for fetching a visit by ID
/// </summary>
public interface IGetVisitByIdHandler : IRequestHandler<GetVisitByIdRequest, GetVisitByIdResponse>
{ }

/// <summary>
/// Handler implementation for fetching a provider by ID
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="visitRepository"></param>
public class GetVisitByIdHandler(ILogger<GetVisitByIdHandler> logger, IValidator<GetVisitByIdRequest> validator, IVisitRepository visitRepository)
  : BaseRequestHandler<GetVisitByIdRequest, GetVisitByIdResponse>(validator), IGetVisitByIdHandler
{
  private readonly ILogger<GetVisitByIdHandler> _logger = logger;
  private readonly IVisitRepository _visitRepository = visitRepository;

  /// <summary>
  /// Handles the retrieval of a visit by ID
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns><see cref="GetVisitByIdResponse"/></returns>
  protected override async Task<ApiResponse<GetVisitByIdResponse>> HandleRequest(GetVisitByIdRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling GetVisitByIdRequest for Visit ID: {Id}", request.Id);

    var visit = await _visitRepository.GetVisitById(request.Id, request.IncludeInvoice, cancellationToken);

    if (visit == null)
    {
      _logger.LogWarning("No visit found with ID: {Id}", request.Id);
    }

    _logger.LogDebug("Retrieved visit details for Visit ID: {Id}", request.Id);

    return ApiResponse<GetVisitByIdResponse>.Succeed(new GetVisitByIdResponse(visit?.ToDto() ?? null));
  }
}