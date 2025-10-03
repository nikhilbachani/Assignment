using PracticeApi.Features.Invoices.GetInvoiceById;
using PracticeApi.Features.Invoices.PayInvoice;
using PracticeApi.Shared;

namespace PracticeApi.Features.Invoices;

/// <summary>
/// Defines the endpoints for managing invoices
/// </summary>
public static class InvoiceEndpoints
{
  /// <summary>
  /// Maps the invoice-related endpoints to the application
  /// </summary>
  public static void MapInvoiceEndpoints(this WebApplication app)
  {
    var invoiceEndpoints = app.MapGroup("/api/invoices")
      .WithTags("invoices");

    invoiceEndpoints
      .MapGet("/{id:int}", async (int id, IGetInvoiceByIdHandler handler, CancellationToken cancellationToken) => await GetInvoiceById(id, handler, cancellationToken))
      .WithName("GetInvoiceById")
      .Produces<ApiResponse<GetInvoiceByIdResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
      .Produces<ApiResponse>(StatusCodes.Status404NotFound);

    invoiceEndpoints
      .MapPut("/pay", async (PayInvoiceRequest request, IPayInvoiceHandler handler, CancellationToken cancellationToken) => await PayInvoice(request, handler, cancellationToken))
      .WithName("PayInvoice")
      .Produces<ApiResponse<PayInvoiceResponse>>(StatusCodes.Status200OK)
      .Produces<ApiResponse>(StatusCodes.Status400BadRequest);
  }

  #region Endpoint Handlers

  private static async Task<IResult> GetInvoiceById(int id, IGetInvoiceByIdHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(new GetInvoiceByIdRequest(id), cancellationToken);

    if (response.Body?.Invoice == null)
    {
      return Results.NotFound();
    }

    return Results.Ok(response.Body.Invoice);
  }

  private static async Task<IResult> PayInvoice(PayInvoiceRequest request, IPayInvoiceHandler handler, CancellationToken cancellationToken = default)
  {
    var response = await handler.Handle(request, cancellationToken);

    if (!response.Success)
    {
      return Results.BadRequest(response.Errors);
    }

    return Results.Ok(response.Body);
  }

  #endregion
}
