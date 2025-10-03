using FluentValidation;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;
using PracticeApi.Infrastructure.Security;
using PracticeApi.Shared;

namespace PracticeApi.Features.Patients.AddPatient;

/// <summary>
/// Handler interface for adding a new patient
/// </summary>
public interface IAddPatientHandler : IRequestHandler<AddPatientRequest, AddPatientResponse>
{}

/// <summary>
/// Handler implementation for <see cref="IAddPatientHandler"/>
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="repository"></param>
/// <param name="encryptionService"></param>
public class AddPatientHandler(ILogger<AddPatientHandler> logger, IValidator<AddPatientRequest> validator, IPatientRepository repository, IEncryptionService encryptionService)
  : BaseRequestHandler<AddPatientRequest, AddPatientResponse>(validator), IAddPatientHandler
{
  private readonly ILogger<AddPatientHandler> _logger = logger;
  private readonly IPatientRepository _patientRepository = repository;
  private readonly IEncryptionService _encryptionService = encryptionService;

  /// <summary>
  /// Handles the addition of a new patient
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns><see cref="AddPatientResponse"/></returns>
  protected override async Task<ApiResponse<AddPatientResponse>> HandleRequest(AddPatientRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling AddPatientRequest for {FirstName} {LastName}", request.FirstName, request.LastName);

    var newPatient = new Patient
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      DOB = request.DOB,
      Email = request.Email,
      Phone = request.Phone,
      SSN = _encryptionService.Encrypt(request.SSN)
    };

    var id = await _patientRepository.AddPatient(newPatient, cancellationToken);
    
    return ApiResponse<AddPatientResponse>.Succeed(new AddPatientResponse(id));
  }
}
