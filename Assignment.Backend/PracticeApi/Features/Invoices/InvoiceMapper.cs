using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Features.Invoices;

/// <summary>
/// Mapper class for <see cref="Invoice"/> and <see cref="InvoiceDto"/>
/// </summary>
public static class InvoiceMapper
{
  /// <summary>
  /// Maps an <see cref="Invoice"/> entity to an <see cref="InvoiceDto"/>
  /// </summary>
  /// <param name="invoice"></param>
  /// <returns></returns>
  public static InvoiceDto ToDto(this Invoice invoice)
  {
    ArgumentNullException.ThrowIfNull(invoice, nameof(invoice));

    return new InvoiceDto
    {
      Amount = invoice.Amount,
      InvoiceDate = invoice.InvoiceDate,
      Status = invoice.PaymentStatus.ToString(),
      PaymentDate = invoice.Receipt?.PaymentDate,
      PaymentMethod = invoice.Receipt?.PaymentMethod.ToString(),
      ReceiptIdentifier = invoice.Receipt?.ReceiptIdentifier
    };
  }
}