namespace PracticeApi.Infrastructure.Data.Models;

/// <summary>
/// Model representing an import job
/// </summary>
public class ImportJob
{
  /// <summary>
  /// Gets or sets the unique ID for the import job
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Gets or sets the public identifier for the import job
  /// </summary>
  public Guid JobIdentifier { get; set; }

  /// <summary>
  /// Gets or sets the name of the file being imported
  /// </summary>
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the status of the import job
  /// </summary>
  public JobStatus Status { get; set; } = JobStatus.Queued;

  /// <summary>
  /// Gets or sets the error message, if any, for the import job
  /// </summary>
  public string? ErrorMessage { get; set; }

  /// <summary>
  /// Gets or sets the creation date and time of the import job
  /// </summary>
  public DateTime CreatedAt { get; set; }

  /// <summary>
  /// Gets or sets the completion date and time of the import job, if completed
  /// </summary>
  public DateTime? CompletedAt { get; set; }
}
