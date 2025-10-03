namespace PracticeApi.Features.Visits.GetVisitById;

/// <summary>
/// Request to get a visit by its ID
/// </summary>
/// <param name="Id"></param>
/// <param name="IncludeInvoice">Whether to include invoice details</param>
public record GetVisitByIdRequest(int Id, bool? IncludeInvoice = false);
