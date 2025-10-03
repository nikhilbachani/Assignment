using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;
using PracticeApi.Shared;

namespace PracticeApi.Features.Visits.AddVisit;

/// <summary>
/// Handler interface for adding a new visit
/// </summary>
public interface IAddVisitHandler : IRequestHandler<AddVisitRequest, AddVisitResponse>
{ }

/// <summary>
/// Handler implementation for adding a new visit
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="repository"></param>
public class AddVisitHandler(ILogger<AddVisitHandler> logger, IValidator<AddVisitRequest> validator, IVisitRepository repository)
  : BaseRequestHandler<AddVisitRequest, AddVisitResponse>(validator), IAddVisitHandler
{
  private readonly ILogger<AddVisitHandler> _logger = logger;
  private readonly IVisitRepository _visitRepository = repository;

  /// <summary>
  /// Handles the addition of a new visit
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<AddVisitResponse>> HandleRequest(AddVisitRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Processing AddVisitRequest for PatientId: {PatientId} with ProviderId: {ProviderId} on {VisitDate} at {VisitTime}.",
      request.PatientId, request.ProviderId, request.VisitDate, request.VisitTime);

    var visit = new Visit
    {
      PatientId = request.PatientId,
      ProviderId = request.ProviderId,
      VisitDate = request.VisitDate,
      VisitTime = request.VisitTime,
      Notes = request.Notes ?? string.Empty
    };

    var visitId = await _visitRepository.AddVisit(visit, cancellationToken);

    return ApiResponse<AddVisitResponse>.Succeed(new AddVisitResponse(visitId));
  }
}
