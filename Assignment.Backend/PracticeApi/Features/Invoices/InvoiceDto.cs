namespace PracticeApi.Features.Invoices;

/// <summary>
/// Data Transfer Object (DTO) for Invoice
/// </summary>
public class InvoiceDto
{
  /// <summary>
  /// Gets or sets the amount for the invoice
  /// </summary>
  public decimal Amount { get; set; }

  /// <summary>
  /// Gets or sets the date of the invoice
  /// </summary>
  public DateOnly InvoiceDate { get; set; }

  /// <summary>
  /// Gets or sets the date of the payment
  /// </summary>
  public DateOnly? PaymentDate { get; set; }

  /// <summary>
  /// Gets or sets the payment method used
  /// </summary>
  public string? PaymentMethod { get; set; }

  /// <summary>
  /// Gets or sets the status of the invoice
  /// </summary>
  public string Status { get; set; } = string.Empty;

  /// <summary>
  ///  Gets or sets the receipt identifier
  /// </summary>
  public string? ReceiptIdentifier { get; set; }
}
