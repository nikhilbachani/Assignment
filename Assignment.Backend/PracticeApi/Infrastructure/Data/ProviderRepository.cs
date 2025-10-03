using Microsoft.EntityFrameworkCore;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Infrastructure.Data;

/// <summary>
/// Defines the contract for provider repository operations
/// </summary>
public interface IProviderRepository
{
  /// <summary>
  /// Get all providers
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<IReadOnlyCollection<Provider>> GetAllProviders(CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets a provider by its ID
  /// </summary>
  /// <param name="id"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<Provider?> GetProviderById(int id, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets a provider by its NPI ID
  /// </summary>
  /// <param name="npiId"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<Provider?> GetProviderByNpiId(string npiId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Adds a new provider
  /// </summary>
  /// <param name="provider"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<int> AddProvider(Provider provider, CancellationToken cancellationToken = default);
}

/// <summary>
/// Provider repository implementation for managing providers
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ProviderRepository"/> class
/// </remarks>
/// <param name="logger"></param>
/// <param name="dbContext"></param>
public class ProviderRepository(ILogger<ProviderRepository> logger, PracticeDbContext dbContext) : IProviderRepository
{
  private readonly ILogger<ProviderRepository> _logger = logger;
  private readonly PracticeDbContext _dbContext = dbContext;

  /// <inheritdoc cref="IProviderRepository.GetAllProviders"/>
  public async Task<IReadOnlyCollection<Provider>> GetAllProviders(CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Fetching all providers from repository.");

    return await _dbContext.Providers.ToListAsync(cancellationToken);
  }

  /// <inheritdoc cref="IProviderRepository.GetProviderById"/>
  public async Task<Provider?> GetProviderById(int id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Fetching provider with ID: {Id} from repository.", id);

    return await _dbContext.Providers.FindAsync([id], cancellationToken);
  }

  /// <inheritdoc cref="IProviderRepository.GetProviderByNpiId"/>
  public async Task<Provider?> GetProviderByNpiId(string npiId, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Fetching provider with NPI ID: {NpiId} from repository.", npiId);

    return await _dbContext.Providers.FirstOrDefaultAsync(p => p.NpiId == npiId, cancellationToken);
  }

  /// <inheritdoc cref="IProviderRepository.AddProvider"/>
  public async Task<int> AddProvider(Provider provider, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Trying to add provider with NPI ID: {NpiId} to repository.", provider.NpiId);

    var existingProvider = await GetProviderByNpiId(provider.NpiId, cancellationToken);
    if (existingProvider != null)
    {
      _logger.LogWarning("Provider with NPI ID {NpiId} already exists in repository. Skipped adding duplicate entry.", provider.NpiId);

      return existingProvider.Id;
    }

    var newProvider = await _dbContext.Providers.AddAsync(provider, cancellationToken);

    await _dbContext.SaveChangesAsync(cancellationToken);
    _logger.LogInformation("Provider with NPI ID: {NpiId} added to repository.", provider.NpiId);

    return newProvider.Entity.Id;
  }
}
