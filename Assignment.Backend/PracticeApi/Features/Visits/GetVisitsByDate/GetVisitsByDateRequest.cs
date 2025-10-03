namespace PracticeApi.Features.Visits.GetVisitsByDate;

/// <summary>
/// Request to get visits by date
/// </summary>
/// <param name="VisitDate"></param>
/// <param name="ProviderId"></param>
/// <param name="IncludeInvoice"></param>
public record GetVisitsByDateRequest(DateOnly VisitDate, int? ProviderId, bool? IncludeInvoice);
