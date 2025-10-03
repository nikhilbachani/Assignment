namespace PracticeApi.Features.Providers.GetProviderById;

/// <summary>
/// Response model for <see cref="GetProviderByIdRequest"/>
/// </summary>
/// <param name="Provider"></param>
public record GetProviderByIdResponse(ProviderDto? Provider);
