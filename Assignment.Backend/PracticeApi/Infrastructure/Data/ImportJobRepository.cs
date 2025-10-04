using Microsoft.EntityFrameworkCore;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Infrastructure.Data;

/// <summary>
/// Repository interface for managing import jobs
/// </summary>
public interface IImportJobRepository
{
  /// <summary>
  /// Creates a new import job with the specified file name
  /// </summary>
  /// <param name="fileName"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<Guid> SubmitImportJob(string fileName, CancellationToken cancellationToken);

  /// <summary>
  /// Retrieves an import job by its public identifier
  /// </summary>
  /// <param name="publicId"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<ImportJob?> GetImportJobByPublicId(Guid publicId, CancellationToken cancellationToken);
}

/// <summary>
/// Repository implementation for <see cref="IImportJobRepository"/>
/// </summary>
/// <param name="dbContext"></param>
public class ImportJobRepository(PracticeDbContext dbContext) : IImportJobRepository
{
  private readonly PracticeDbContext _dbContext = dbContext;

  /// <inheritdoc cref="IImportJobRepository.SubmitImportJob"/>
  public async Task<Guid> SubmitImportJob(string fileName, CancellationToken cancellationToken)
  {
    var importJob = new ImportJob
    {
      JobIdentifier = Guid.NewGuid(),
      FileName = fileName,
      Status = JobStatus.Queued,
      CreatedAt = DateTime.Now
    };

    _dbContext.ImportJobs.Add(importJob);
    await _dbContext.SaveChangesAsync(cancellationToken);

    return importJob.JobIdentifier;
  }

  /// <inheritdoc cref="IImportJobRepository.GetImportJobByPublicId"/>
  public async Task<ImportJob?> GetImportJobByPublicId(Guid publicId, CancellationToken cancellationToken)
  {
    return await _dbContext.ImportJobs
      .FirstOrDefaultAsync(j => j.JobIdentifier == publicId, cancellationToken);
  }
}