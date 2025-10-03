namespace PracticeApi.Features.Providers.GetProviderByNpiId;

/// <summary>
/// Response model for <see cref="GetProviderByNpiIdRequest"/>
/// </summary>
/// <param name="Provider"></param>
public record GetProviderByNpiIdResponse(ProviderDto? Provider);