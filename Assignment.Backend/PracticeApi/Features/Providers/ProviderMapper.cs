using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Features.Providers;

/// <summary>
/// Mapper class for <see cref="Provider"/> and <see cref="ProviderDto"/>
/// </summary>
public static class ProviderMapper
{
  /// <summary>
  /// Maps an <see cref="Provider"/> entity to an <see cref="ProviderDto"/>
  /// </summary>
  /// <param name="provider"></param>
  /// <returns></returns>
  public static ProviderDto ToDto(this Provider provider)
  {
    ArgumentNullException.ThrowIfNull(provider, nameof(provider));

    return new ProviderDto
    {
      ProviderId = provider.Id,
      ProviderName = provider.FirstName + " " + provider.LastName,
    };
  }
}
