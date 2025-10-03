namespace PracticeApi.Features.Patients.GetPatientById;

/// <summary>
/// Response model for <see cref="GetPatientByIdRequest"/>
/// </summary>
/// <param name="Patient"></param>
public record GetPatientByIdResponse(PatientDto? Patient);
