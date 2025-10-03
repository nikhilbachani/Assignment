namespace PracticeApi.Features.Invoices.PayInvoice;

/// <summary>
/// Request to pay an invoice
/// </summary>
/// <param name="InvoiceId"></param>
/// <param name="PaymentMethod"></param>
public record PayInvoiceRequest(int InvoiceId, string PaymentMethod);
