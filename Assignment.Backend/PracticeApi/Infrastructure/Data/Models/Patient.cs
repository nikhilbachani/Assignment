using CsvHelper.Configuration.Attributes;

namespace PracticeApi.Infrastructure.Data.Models;

/// <summary>
/// Represents a patient in the system
/// </summary>
public class Patient
{
  /// <summary>
  /// Gets or sets the unique ID for the patient
  /// </summary>
  [Ignore]
  public int Id { get; set; }

  /// <summary>
  /// Gets or sets the patient's first name
  /// </summary>
  public string FirstName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the patient's last name
  /// </summary>
  public string LastName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the patient's date of birth
  /// </summary>
  public DateOnly DOB { get; set; }

  /// <summary>
  /// Gets or sets the patient's email address
  /// </summary>
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the patient's phone number
  /// </summary>
  public string Phone { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the patient's social security number
  /// </summary>
  public string SSN { get; set; } = string.Empty;
}