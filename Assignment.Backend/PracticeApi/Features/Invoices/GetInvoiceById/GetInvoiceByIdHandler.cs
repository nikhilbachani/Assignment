using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Invoices.GetInvoiceById;

/// <summary>
/// Handler interface for fetching an invoice by ID
/// </summary>
public interface IGetInvoiceByIdHandler : IRequestHandler<GetInvoiceByIdRequest, GetInvoiceByIdResponse>
{ }

/// <summary>
/// Handler for <see cref="GetInvoiceByIdRequest"/>
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="invoiceRepository"></param>
public class GetInvoiceByIdHandler(ILogger<GetInvoiceByIdHandler> logger, IValidator<GetInvoiceByIdRequest> validator, IInvoiceRepository invoiceRepository)
  : BaseRequestHandler<GetInvoiceByIdRequest, GetInvoiceByIdResponse>(validator), IGetInvoiceByIdHandler
{
  private readonly ILogger<GetInvoiceByIdHandler> _logger = logger;
  private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;

  /// <summary>
  /// Handles the retrieval of an invoice by ID
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns><see cref="GetInvoiceByIdResponse"/></returns>
  protected override async Task<ApiResponse<GetInvoiceByIdResponse>> HandleRequest(GetInvoiceByIdRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Handling GetInvoiceByIdRequest for Invoice ID: {Id}", request.Id);

    var invoice = await _invoiceRepository.GetInvoiceById(request.Id, cancellationToken);

    if (invoice == null)
    {
      _logger.LogWarning("No invoice found with ID: {Id}", request.Id);
    }

    return ApiResponse<GetInvoiceByIdResponse>.Succeed(new GetInvoiceByIdResponse(invoice?.ToDto() ?? null));
  }
}