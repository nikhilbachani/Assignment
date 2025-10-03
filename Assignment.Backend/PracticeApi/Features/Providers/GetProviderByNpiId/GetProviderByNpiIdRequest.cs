using Microsoft.AspNetCore.Mvc;

namespace PracticeApi.Features.Providers.GetProviderByNpiId;

/// <summary>
/// Request DTO to get a provider by NPI ID
/// </summary>
/// <param name="NpiId"></param>
public record GetProviderByNpiIdRequest(string NpiId);
