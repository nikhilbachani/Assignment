namespace PracticeApi.Features.Providers;

/// <summary>
/// Common constants related to the Providers feature
/// </summary>
public static class ProviderConstants
{
  /// <summary>
  /// Regex pattern for validating a 10-digit NPI ID
  /// </summary>
  public const string NpiRegex = @"^\d{10}$";
}
