using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Patients.SearchPatient;

/// <summary>
/// Handler interface for <see cref="SearchPatientRequest"/>
/// </summary>
public interface ISearchPatientHandler : IRequestHandler<SearchPatientRequest, SearchPatientResponse>
{ }

/// <summary>
/// Handler implementation for <see cref="ISearchPatientHandler"/>
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="patientRepository"></param>
public class SearchPatientHandler(ILogger<SearchPatientHandler> logger, IValidator<SearchPatientRequest> validator, IPatientRepository patientRepository)
  : BaseRequestHandler<SearchPatientRequest, SearchPatientResponse>(validator), ISearchPatientHandler
{
  private readonly ILogger<SearchPatientHandler> _logger = logger;
  private readonly IPatientRepository _patientRepository = patientRepository;

  /// <summary>
  /// Handles the retrieval of patients by search term
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<SearchPatientResponse>> HandleRequest(SearchPatientRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling SearchPatientRequest for term: {SearchTerm}", request.SearchTerm);

    var patients = await _patientRepository.SearchPatients(request.SearchTerm, cancellationToken);

    if (patients.Count == 0)
    {
      _logger.LogWarning("No patients found matching search term: {SearchTerm}", request.SearchTerm);
    }

    return ApiResponse<SearchPatientResponse>.Succeed(new SearchPatientResponse(patients.Select(p => p.ToDto()).ToList()));
  }
}

