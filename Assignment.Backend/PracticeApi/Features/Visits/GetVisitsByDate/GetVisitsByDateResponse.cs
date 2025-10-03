namespace PracticeApi.Features.Visits.GetVisitsByDate;

/// <summary>
/// Response model for <see cref="GetVisitsByDateRequest"/>
/// </summary>
/// <param name="Visits"></param>
public record GetVisitsByDateResponse(IReadOnlyCollection<VisitDto> Visits);
