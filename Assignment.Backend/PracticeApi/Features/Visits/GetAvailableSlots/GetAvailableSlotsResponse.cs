namespace PracticeApi.Features.Visits.GetAvailableSlots;

/// <summary>
/// Response model for <see cref="GetAvailableSlotsRequest"/>
/// </summary>
/// <param name="AvailableSlots"></param>
public record GetAvailableSlotsResponse(IReadOnlyCollection<TimeOnly> AvailableSlots);
