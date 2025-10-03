namespace PracticeApi.Features.Providers.AddProvider;

/// <summary>
/// Request DTO to add a new provider
/// </summary>
public record AddProviderRequest(string FirstName, string LastName, string Specialty, string NpiId);
