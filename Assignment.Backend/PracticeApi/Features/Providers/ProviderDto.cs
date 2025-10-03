namespace PracticeApi.Features.Providers;

/// <summary>
/// Data Transfer Object (DTO) for Provider
/// </summary>
public class ProviderDto
{
  /// <summary>
  /// Gets or sets the provider's unique identifier
  /// </summary>
  public int ProviderId { get; set; }

  /// <summary>
  /// Gets or sets the provider's full name
  /// </summary>
  public string ProviderName { get; set; } = string.Empty;
}
