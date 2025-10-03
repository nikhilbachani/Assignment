namespace PracticeApi.Features.Visits.AddVisit;

/// <summary>
/// Request DTO to add a new visit
/// </summary>
/// <param name="VisitDate"></param>
/// <param name="VisitTime"></param>
/// <param name="ProviderId"></param>
/// <param name="PatientId"></param>
/// <param name="Notes"></param>
public record AddVisitRequest(DateOnly VisitDate, TimeOnly VisitTime, int ProviderId, int PatientId, string? Notes);
