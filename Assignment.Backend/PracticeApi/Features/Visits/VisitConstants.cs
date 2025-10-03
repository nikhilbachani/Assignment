namespace PracticeApi.Features.Visits;

/// <summary>
/// Constants related to the Visits feature
/// </summary>
public static class VisitConstants
{
  /// <summary>
  /// Clinic opening hour (9 AM)
  /// </summary>
  public const int ClinicOpenHour = 9;

  /// <summary>
  /// Clinic closing hour (5 PM)
  /// </summary>
  public const int ClinicCloseHour = 17;

  /// <summary>
  /// Slot duration in minutes (15 minutes)
  /// </summary>
  public const int SlotDurationMinutes = 15;

  /// <summary>
  /// Standard visit fee
  /// </summary>
  public const decimal StandardVisitFee = 100.00m;

  /// <summary>
  /// Time format for visit times
  /// </summary>
  public const string TimeFormat = "t";
}
