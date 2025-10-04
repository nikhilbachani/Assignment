using CsvHelper;
using System.Threading.Channels;
using System.Globalization;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;
using PracticeApi.Infrastructure.Security;

namespace PracticeApi.Infrastructure.BackgroundServices;

/// <summary>
/// Background service to handle CSV import requests
/// </summary>
public class CsvImportService(Channel<string> filePathQueue, IServiceProvider serviceProvider, ILogger<CsvImportService> logger) : BackgroundService
{
  private readonly Channel<string> _filePathQueue = filePathQueue;
  private readonly IServiceProvider _serviceProvider = serviceProvider;
  private readonly ILogger<CsvImportService> _logger = logger;

  /// <summary>
  /// Executes the background service to process CSV import requests
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    while (!cancellationToken.IsCancellationRequested)
    {
      try
      {
        var filePath = await _filePathQueue.Reader.ReadAsync(cancellationToken);
        _logger.LogDebug("Processing file: {FilePath}", filePath);

        // Resolve PracticeDbContext (scoped) in background service (singleton)
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PracticeDbContext>();
        var encryptionService = scope.ServiceProvider.GetRequiredService<IEncryptionService>();

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Patient>()
          .Select(p =>
          {
            // Encrypt sensitive data before storing in database
            p.SSN = encryptionService.Encrypt(p.SSN);
            return p;
          }).ToList();

        dbContext.Patients.AddRange(records);
        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully processed file: {FilePath}", filePath);

        DeleteFile(filePath);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error processing file.");
      }
    }
  }

  #region Private Methods

  private static void DeleteFile(string filePath)
  {
    if (File.Exists(filePath))
    {
      File.Delete(filePath);
    }
  }

  #endregion

}