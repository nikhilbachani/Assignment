namespace PracticeApi.Infrastructure.Data.Models;

/// <summary>
/// Represents a payment receipt
/// </summary>
public class Receipt
{
  /// <summary>
  /// Gets or sets the unique ID for the receipt
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Gets or sets the unique receipt identifier
  /// </summary>
  public string ReceiptIdentifier { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the identifier of the associated invoice
  /// </summary>
  public int InvoiceId { get; set; } // Foreign key to Invoice

  /// <summary>
  /// Gets or sets the payment amount
  /// </summary>
  public decimal Amount { get; set; }

  /// <summary>
  /// Gets or sets the date of payment
  /// </summary>
  public DateOnly PaymentDate { get; set; }

  /// <summary>
  /// Gets or sets the payment method used
  /// </summary>
  public PaymentMode PaymentMethod { get; set; } = PaymentMode.Cash;
}