using Microsoft.AspNetCore.Mvc;
using PracticeApi.Features.Visits.AddVisit;
using PracticeApi.Features.Visits.GetAvailableSlots;
using PracticeApi.Features.Visits.GetVisitById;
using PracticeApi.Features.Visits.GetVisitsByDate;
using PracticeApi.Shared;

namespace PracticeApi.Features.Visits;

/// <summary>
/// Defines the endpoints for managing visits
/// </summary>
public static class VisitEndpoints
{
  /// <summary>
  /// Maps the visit-related endpoints to the application
  /// </summary>
  public static void MapVisitEndpoints(this WebApplication app)
  {
    var visitEndpoints = app.MapGroup("/api/visits")
      .WithTags("visits");

    visitEndpoints
      .MapGet("/", async (DateOnly visitDate, int? providerId, bool? includeInvoice, [FromServices] IGetVisitsByDateHandler handler, CancellationToken cancellationToken) => await GetVisitsByDate(visitDate, providerId, includeInvoice, handler, cancellationToken))
      .WithName("GetVisitsByDate")
      .Produces<ApiResponse<GetVisitsByDateResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound)
      .WithOpenApi();

    visitEndpoints
      .MapGet("/{id:int}", async (int id, bool? includeInvoice, [FromServices] IGetVisitByIdHandler handler, CancellationToken cancellationToken) => await GetVisitById(id, includeInvoice, handler, cancellationToken))
      .WithName("GetVisitById")
      .Produces<ApiResponse<GetVisitByIdResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound)
      .WithOpenApi();

    visitEndpoints
      .MapGet("/slots", async (DateOnly visitDate, int providerId, [FromServices] IGetAvailableSlotsHandler handler, CancellationToken cancellationToken) => await GetAvailableSlots(visitDate, providerId, handler, cancellationToken))
      .WithName("GetAvailableSlots")
      .Produces<ApiResponse<GetAvailableSlotsResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound)
      .WithOpenApi();

    visitEndpoints
      .MapPost("/", async (AddVisitRequest request, [FromServices] IAddVisitHandler handler, CancellationToken cancellationToken) => await AddVisit(request, handler, cancellationToken))
      .WithName("AddVisit")
      .Produces<ApiResponse<AddVisitResponse>>(StatusCodes.Status201Created)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest);
  }

  #region Endpoint Handlers

  private static async Task<IResult> GetVisitsByDate(DateOnly visitDate, int? providerId, bool? includeInvoice, IGetVisitsByDateHandler handler, CancellationToken cancellationToken = default)
  {
    var request = new GetVisitsByDateRequest(visitDate, providerId, includeInvoice);

    var response = await handler.Handle(request, cancellationToken);

    if (response.Body?.Visits == null)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body.Visits);
  }

  private static async Task<IResult> GetVisitById(int id, bool? includeInvoice, IGetVisitByIdHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(new GetVisitByIdRequest(id, includeInvoice), cancellationToken);

    if (response.Body?.Visit == null)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body.Visit);
  }

  private static async Task<IResult> GetAvailableSlots(DateOnly visitDate, int providerId, IGetAvailableSlotsHandler handler, CancellationToken cancellationToken = default)
  {
    var request = new GetAvailableSlotsRequest(visitDate, providerId);

    var response = await handler.Handle(request, cancellationToken);

    if (response.Body?.AvailableSlots?.Count == 0)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body?.AvailableSlots);
  }

  private static async Task<IResult> AddVisit(AddVisitRequest request, IAddVisitHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(request, cancellationToken);

    return Results.Created($"/api/visits/{response.Body?.VisitId}", response.Body);
  }

  #endregion
}
