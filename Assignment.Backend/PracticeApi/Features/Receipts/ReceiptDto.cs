namespace PracticeApi.Features.Receipts;

/// <summary>
/// Data transfer object for receipt information
/// </summary>
public class ReceiptDto
{
  /// <summary>
  /// Receipt identifier
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Associated invoice identifier
  /// </summary>
  public int InvoiceId { get; set; }

  /// <summary>
  /// Payment amount
  /// </summary>
  public decimal Amount { get; set; }

  /// <summary>
  /// Date of payment
  /// </summary>
  public DateOnly PaymentDate { get; set; }

  /// <summary>
  /// Payment method used
  /// </summary>
  public string PaymentMethod { get; set; } = string.Empty;
}
