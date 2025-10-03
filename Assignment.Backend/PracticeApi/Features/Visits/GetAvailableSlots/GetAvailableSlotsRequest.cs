namespace PracticeApi.Features.Visits.GetAvailableSlots;

/// <summary>
/// Request to get available slots for a provider on a specific date
/// </summary>
/// <param name="VisitDate"></param>
/// <param name="ProviderId"></param>
public record GetAvailableSlotsRequest(DateOnly VisitDate, int ProviderId);
