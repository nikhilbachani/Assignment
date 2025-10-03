namespace PracticeApi.Features.Invoices.PayInvoice;

/// <summary>
/// Response model for <see cref="PayInvoiceRequest"/>
/// </summary>
/// <param name="ReceiptIdentifier"></param>
public record PayInvoiceResponse(string ReceiptIdentifier);
