namespace PracticeApi.Features.Invoices.GetInvoiceById;

/// <summary>
/// Response model for <see cref="GetInvoiceByIdRequest"/>
/// </summary>
/// <param name="Invoice"></param>
public record GetInvoiceByIdResponse(InvoiceDto? Invoice);
