namespace PracticeApi.Features.Patients.ImportPatients;

/// <summary>
/// Request to import patients from a CSV file
/// </summary>
/// <param name="File"></param>
public record ImportPatientsRequest(IFormFile File);
