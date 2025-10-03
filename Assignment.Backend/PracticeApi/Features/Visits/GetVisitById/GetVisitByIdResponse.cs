namespace PracticeApi.Features.Visits.GetVisitById;

/// <summary>
/// Response model for <see cref="GetVisitByIdRequest"/>
/// </summary>
/// <param name="Visit"></param>
public record GetVisitByIdResponse(VisitDto? Visit);
