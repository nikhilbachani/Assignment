using Microsoft.AspNetCore.Mvc;
using PracticeApi.Features.Patients.AddPatient;
using PracticeApi.Features.Patients.GetPatientById;
using PracticeApi.Features.Patients.ImportPatients;
using PracticeApi.Features.Patients.SearchPatient;
using PracticeApi.Shared;

namespace PracticeApi.Features.Patients;

/// <summary>
/// Defines the endpoints for managing patients
/// </summary>
public static class PatientEndpoints
{
  /// <summary>
  /// Maps the patient-related endpoints to the application
  /// </summary>
  public static void MapPatientEndpoints(this WebApplication app)
  {
    var patientEndpoints = app.MapGroup("/api/patients")
      .WithTags("patients");

    patientEndpoints
      .MapGet("/{id:int}", async (int id, [FromServices] IGetPatientByIdHandler handler, CancellationToken cancellationToken) => await GetPatientById(id, handler, cancellationToken))
      .WithName("GetPatientById")
      .Produces<ApiResponse<GetPatientByIdResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound);

    patientEndpoints
      .MapGet("/search", async (string searchTerm, [FromServices] ISearchPatientHandler handler, CancellationToken cancellationToken) => await SearchPatients(searchTerm, handler, cancellationToken))
      .WithName("SearchPatients")
      .Produces<ApiResponse<SearchPatientResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound);

    patientEndpoints
      .MapPost("/", async (AddPatientRequest request, [FromServices] IAddPatientHandler handler, CancellationToken cancellationToken) => await AddPatient(request, handler, cancellationToken))
      .WithName("AddPatient")
      .Produces<ApiResponse<AddPatientResponse>>(StatusCodes.Status201Created)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest);

    patientEndpoints
      .MapPost("/bulk-import", async (IFormFile file, [FromServices] IImportPatientsHandler handler, CancellationToken cancellationToken) => await BulkImportPatients(file, handler, cancellationToken))
      .WithName("BulkImportPatients")
      .Accepts<IFormFile>("multipart/form-data")
      .DisableAntiforgery()
      .Produces<ApiResponse<ImportPatientsResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest);
  }

  #region Endpoint Handlers

  private static async Task<IResult> GetPatientById(int id, IGetPatientByIdHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(new GetPatientByIdRequest(id), cancellationToken);

    if (response.Body?.Patient == null)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body.Patient);
  }

  private static async Task<IResult> SearchPatients(string searchTerm, ISearchPatientHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(new SearchPatientRequest(searchTerm), cancellationToken);

    if (response.Body?.Patients == null)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body.Patients);
  }

  private static async Task<IResult> AddPatient(AddPatientRequest request, IAddPatientHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(request, cancellationToken);

    return Results.Created($"/api/patients/{response.Body?.PatientId}", response.Body);
  }

  private static async Task<IResult> BulkImportPatients(IFormFile file, IImportPatientsHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(new ImportPatientsRequest(file), cancellationToken);

    if (!response.Success || response.Body == null)
    {
      return Results.InternalServerError(response);
    }

    return Results.Ok(response.Body);
  }

  #endregion
}
