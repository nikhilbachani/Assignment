namespace PracticeApi.Features.Providers.GetProviders;

/// <summary>
/// Response model for <see cref="GetProvidersRequest"/>
/// </summary>
/// <param name="Providers"></param>
public record GetProvidersResponse(IReadOnlyCollection<ProviderDto> Providers);
