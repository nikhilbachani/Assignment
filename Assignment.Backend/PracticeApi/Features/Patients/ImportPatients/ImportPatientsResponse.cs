namespace PracticeApi.Features.Patients.ImportPatients;

/// <summary>
/// Response model for <see cref="ImportPatientsRequest"/>
/// </summary>
/// <param name="JobId">The ID of the import job</param>
public record ImportPatientsResponse(Guid JobId);
