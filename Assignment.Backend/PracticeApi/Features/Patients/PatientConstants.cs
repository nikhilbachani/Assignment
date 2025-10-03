namespace PracticeApi.Features.Patients;

/// <summary>
/// Constants related to patients
/// </summary>
public static class PatientConstants
{
  /// <summary>
  /// Maximum length for first and last names
  /// </summary>
  public const int MaxNameLength = 50;

  /// <summary>
  /// Regular expression for validating phone numbers
  /// </summary>
  public const string PhoneRegex = @"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$";

  /// <summary>
  /// Regular expression for validating SSN
  /// </summary>
  public const string SsnRegex = @"^\d{3}[-\s]?\d{2}[-\s]?\d{4}$";
}