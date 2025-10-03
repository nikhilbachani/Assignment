namespace PracticeApi.Features.Patients;

/// <summary>
/// Data Transfer Object (DTO) for Patient
/// </summary>
public class PatientDto
{
  /// <summary>
  /// Gets or sets the patient's unique identifier
  /// </summary>
  public int PatientId { get; set; }

  /// <summary>
  /// Gets or sets the patient's full name
  /// </summary>
  public string PatientName { get; set; } = string.Empty;

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
}