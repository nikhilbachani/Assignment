namespace PracticeApi.Infrastructure.Data.Models;

/// <summary>
/// Represents a healthcare provider
/// </summary>
public class Provider
{
  /// <summary>
  /// Gets or sets the unique ID for the provider
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Gets or sets the first name of the provider
  /// </summary>
  public string FirstName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the last name of the provider
  /// </summary>
  public string LastName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the specialty of the provider
  /// </summary>
  public string Specialty { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the unique national provider identifier (NPI)
  /// </summary>
  public string NpiId { get; set; } = string.Empty;
}