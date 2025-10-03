using FluentValidation;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Invoices.PayInvoice;

/// <summary>
/// Handler interface for paying an invoice
/// </summary>
public interface IPayInvoiceHandler : IRequestHandler<PayInvoiceRequest, PayInvoiceResponse>
{ }


/// <summary>
/// Handler implementation for <see cref="IPayInvoiceHandler" />
/// </summary>
/// <param name="logger"></param>
/// <param name="validator"></param>
/// <param name="repository"></param>
public class PayInvoiceHandler(ILogger<PayInvoiceHandler> logger, IValidator<PayInvoiceRequest> validator, IInvoiceRepository repository)
  : BaseRequestHandler<PayInvoiceRequest, PayInvoiceResponse>(validator), IPayInvoiceHandler
{
  private readonly ILogger<PayInvoiceHandler> _logger = logger;
  private readonly IInvoiceRepository _invoiceRepository = repository;

  /// <summary>
  /// Handles the payment of an invoice
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<PayInvoiceResponse>> HandleRequest(PayInvoiceRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Processing PayInvoiceRequest for InvoiceId: {InvoiceId}.", request.InvoiceId);

    var receiptIdentifier = await _invoiceRepository.PayInvoice(request.InvoiceId, request.PaymentMethod, cancellationToken);
    if (string.IsNullOrWhiteSpace(receiptIdentifier))
    {
      _logger.LogWarning("Failed to pay invoice with ID: {InvoiceId}. It may not exist or is already paid.", request.InvoiceId);
      return new ApiResponse<PayInvoiceResponse> { Success = false, Errors = [ "Failed to pay invoice. It may not exist or is already paid." ] };
    }

    return ApiResponse<PayInvoiceResponse>.Succeed(new PayInvoiceResponse(receiptIdentifier));
  }
}