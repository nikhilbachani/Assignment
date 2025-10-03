using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Security;
using PracticeApi.Shared;

namespace PracticeApi.Features.Patients.GetPatientById;

/// <summary>
/// Handler interface for <see cref="GetPatientByIdRequest"/>
/// </summary>
public interface IGetPatientByIdHandler : IRequestHandler<GetPatientByIdRequest, GetPatientByIdResponse>
{ }

/// <summary>
/// Handler implementation for <see cref="IGetPatientByIdHandler"/>
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="patientRepository"></param>
/// <param name="encryptionService"></param>
public class GetPatientByIdHandler(ILogger<GetPatientByIdHandler> logger, IValidator<GetPatientByIdRequest> validator, IPatientRepository patientRepository, IEncryptionService encryptionService)
  : BaseRequestHandler<GetPatientByIdRequest, GetPatientByIdResponse>(validator), IGetPatientByIdHandler
{
  private readonly ILogger<GetPatientByIdHandler> _logger = logger;
  private readonly IPatientRepository _patientRepository = patientRepository;
  private readonly IEncryptionService _encryptionService = encryptionService;

  /// <summary>
  /// Handles the retrieval of a patient by ID
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<GetPatientByIdResponse>> HandleRequest(GetPatientByIdRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling GetPatientByIdRequest for ID: {Id}", request.Id);

    var patient = await _patientRepository.GetPatientById(request.Id, cancellationToken);

    if (patient == null)
    {
      _logger.LogWarning("No patient found with ID: {Id}", request.Id);
    }
    else
    {
      // Decrypt sensitive information before returning
      patient.SSN = _encryptionService.Decrypt(patient.SSN);
    }

    return ApiResponse<GetPatientByIdResponse>.Succeed(new GetPatientByIdResponse(patient?.ToDto() ?? null));
  }
}
