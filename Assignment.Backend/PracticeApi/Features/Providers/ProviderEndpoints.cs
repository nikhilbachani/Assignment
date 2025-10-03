using Microsoft.AspNetCore.Mvc;

using PracticeApi.Features.Providers.AddProvider;
using PracticeApi.Features.Providers.GetProviderById;
using PracticeApi.Features.Providers.GetProviderByNpiId;
using PracticeApi.Features.Providers.GetProviders;
using PracticeApi.Shared;

namespace PracticeApi.Features.Providers;

/// <summary>
/// Defines the endpoints for managing providers
/// </summary>
public static class ProviderEndpoints
{
  /// <summary>
  /// Maps the provider-related endpoints to the application
  /// </summary>
  public static void MapProviderEndpoints(this WebApplication app)
  {
    var providerEndpoints = app.MapGroup("/api/providers")
      .WithTags("providers");

    providerEndpoints
      .MapGet("/", async (IGetProvidersHandler handler, CancellationToken cancellationToken) => await GetProviders(handler, cancellationToken))
      .WithName("GetProviders")
      .Produces<ApiResponse<GetProvidersResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound);

    providerEndpoints
      .MapGet("/{id:int}", async (int id, [FromServices] IGetProviderByIdHandler handler, CancellationToken cancellationToken) => await GetProviderById(id, handler, cancellationToken))
      .WithName("GetProviderById")
      .Produces<ApiResponse<GetProviderByIdResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound);

    providerEndpoints
      .MapGet("/npi/{npiId}", async (string npiId, [FromServices] IGetProviderByNpiIdHandler handler, CancellationToken cancellationToken) => await GetProviderByNpiId(npiId, handler, cancellationToken))
      .WithName("GetProviderByNpiId")
      .Produces<ApiResponse<GetProviderByNpiIdResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound);

    providerEndpoints
      .MapPost("/", async (AddProviderRequest request, [FromServices] IAddProviderHandler handler, CancellationToken cancellationToken) => await AddProvider(request, handler, cancellationToken))
      .WithName("AddProvider")
      .Produces<ApiResponse<AddProviderResponse>>(StatusCodes.Status201Created)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest);
  }

  #region Endpoint Handlers

  private static async Task<IResult> GetProviders(IGetProvidersHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(new GetProvidersRequest(), cancellationToken);

    if (response.Body?.Providers == null)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body.Providers);
  }

  private static async Task<IResult> GetProviderById(int id, IGetProviderByIdHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(new GetProviderByIdRequest(id), cancellationToken);

    if (response.Body?.Provider == null)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body.Provider);
  }

  private static async Task<IResult> GetProviderByNpiId(string npiId, IGetProviderByNpiIdHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(new GetProviderByNpiIdRequest(npiId), cancellationToken);

    if (response.Body?.Provider == null)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body.Provider);
  }

  private static async Task<IResult> AddProvider(AddProviderRequest request, IAddProviderHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(request, cancellationToken);

    return Results.Created($"/api/providers/{response.Body?.ProviderId}", response.Body);
  }

  #endregion
}
