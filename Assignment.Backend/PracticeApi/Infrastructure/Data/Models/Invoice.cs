using PracticeApi.Features.Invoices;

namespace PracticeApi.Infrastructure.Data.Models;

/// <summary>
/// Represents an invoice for a visit
/// </summary>
public class Invoice
{
  /// <summary>
  /// Gets or sets the unique ID for the invoice
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Gets or sets the identifier of the visit associated with the invoice
  /// </summary>
  public int VisitId { get; set; } = default!; // Foreign key to Visit

  /// <summary>
  /// Gets or sets the amount for the invoice
  /// </summary>
  public decimal Amount { get; set; }

  /// <summary>
  /// Gets or sets the date of the invoice
  /// </summary>
  public DateOnly InvoiceDate { get; set; }

  /// <summary>
  /// Gets or sets the status of the invoice
  /// </summary>
  public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

  /// <summary>
  /// Navigation property to the associated receipt
  /// </summary>
  public Receipt? Receipt { get; set; } = default!;
}
