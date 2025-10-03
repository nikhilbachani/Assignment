namespace PracticeApi.Features.Visits;

/// <summary>
/// Represents a patient visit DTO
/// </summary>
public class VisitDto
{
  /// <summary>
  /// Gets or sets the visit ID
  /// </summary>
  public int VisitId { get; set; }

  /// <summary>
  /// Gets or sets the visit start date-time
  /// </summary>
  public DateOnly VisitDate { get; set; }

  /// <summary>
  /// Gets or sets the visit start time
  /// </summary>
  public TimeOnly VisitTime { get; set; }

  /// <summary>
  /// Gets or sets provider name
  /// </summary>
  public string ProviderName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the patient name
  /// </summary>
  public string PatientName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the notes for the visit
  /// </summary>
  public string Notes { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the visit amount
  /// </summary>
  public decimal? VisitAmount { get; set; }

  /// <summary>
  /// Gets or sets the invoice ID
  /// </summary>
  public int? InvoiceId { get; set; }

  /// <summary>
  /// Gets or sets the receipt identifier
  /// </summary>
  public string? ReceiptIdentifier { get; set; }

  /// <summary>
  /// Gets or sets the payment method
  /// </summary>
  public string? PaymentMethod { get; set; }

  /// <summary>
  /// Gets or sets the payment status
  /// </summary>
  public string? PaymentStatus { get; set; }

  /// <summary>
  /// Gets or sets the payment date
  /// </summary>
  public DateOnly? PaymentDate { get; set; }
}
