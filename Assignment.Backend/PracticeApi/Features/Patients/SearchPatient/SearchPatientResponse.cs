namespace PracticeApi.Features.Patients.SearchPatient;

/// <summary>
/// Response model for <see cref="SearchPatientRequest"/>
/// </summary>
/// <param name="Patients"></param>
public record SearchPatientResponse(IReadOnlyCollection<PatientDto> Patients);
