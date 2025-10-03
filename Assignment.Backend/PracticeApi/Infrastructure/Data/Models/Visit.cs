namespace PracticeApi.Infrastructure.Data.Models;

/// <summary>
/// Represents a patient visit
/// </summary>
public class Visit
{
  /// <summary>
  /// Gets or sets the unique ID for the visit
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Gets or sets the visit start date-time
  /// </summary>
  public DateOnly VisitDate { get; set; }

  /// <summary>
  /// Gets or sets the visit start time
  /// </summary>
  public TimeOnly VisitTime { get; set; }

  /// <summary>
  /// Gets or sets the identifier of the provider associated with the visit
  /// </summary>
  public int ProviderId { get; set; } // Foreign key to Provider

  /// <summary>
  /// Gets or sets the identifier of the patient associated with the visit
  /// </summary>
  public int PatientId { get; set; } // Foreign key to Patient

  /// <summary>
  /// Gets or sets the notes for the visit
  /// </summary>
  public string Notes { get; set; } = string.Empty;

  /// <summary>
  /// Navigation property for the associated provider
  /// </summary>
  public Provider Provider { get; set; } = default!;

  /// <summary>
  /// Navigation property for the associated patient
  /// </summary>
  public Patient Patient { get; set; } = default!;

  /// <summary>
  /// Navigation property for the associated invoice
  /// </summary>
  public Invoice Invoice { get; set; } = default!;
}
