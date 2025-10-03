namespace PracticeApi.Features.Patients.SearchPatient;

/// <summary>
/// Request to search for patients by a search term
/// </summary>
/// <param name="SearchTerm"></param>
public record SearchPatientRequest(string SearchTerm);
