namespace PracticeApi.Infrastructure.Data.Models;

/// <summary>
/// Enumeration representing the various statuses an import job can have
/// </summary>
public enum JobStatus
{
  /// <summary>
  /// The job is queued and waiting to be processed
  /// </summary>
  Queued,

  /// <summary>
  /// The job is currently being processed
  /// </summary>
  Processing,

  /// <summary>
  /// The job has been successfully processed
  /// </summary>
  Processed,

  /// <summary>
  /// The job has failed
  /// </summary>
  Failed
}