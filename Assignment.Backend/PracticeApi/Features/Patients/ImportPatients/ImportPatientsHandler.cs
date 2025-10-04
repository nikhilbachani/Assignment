using System.Threading.Channels;
using FluentValidation;

using PracticeApi.Infrastructure.Data;
using PracticeApi.Shared;

namespace PracticeApi.Features.Patients.ImportPatients;

/// <summary>
/// Handler for <see cref="ImportPatientsRequest"/>
/// </summary>
public interface IImportPatientsHandler : IRequestHandler<ImportPatientsRequest, ImportPatientsResponse>
{ }

/// <summary>
/// Handler implementation for <see cref="IImportPatientsHandler"/>
/// </summary>
public class ImportPatientsHandler(ILogger<ImportPatientsHandler> logger, IValidator<ImportPatientsRequest> validator, IImportJobRepository importJobRepository, Channel<string> filePathQueue)
  : BaseRequestHandler<ImportPatientsRequest, ImportPatientsResponse>(validator), IImportPatientsHandler
{
  private readonly ILogger<ImportPatientsHandler> _logger = logger;
  private readonly IImportJobRepository _importJobRepository = importJobRepository;
  private readonly Channel<string> _filePathQueue = filePathQueue;

  /// <summary>
  /// Handles the request to import patients from a CSV file
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected override async Task<ApiResponse<ImportPatientsResponse>> HandleRequest(ImportPatientsRequest request, CancellationToken cancellationToken)
  {
    _logger.LogDebug("Submitting file: {FileName} for importing patients", request.File.FileName);
    var uniqueFileName = $"{Guid.NewGuid()}_{request.File.FileName}";
    var filePath = Path.Combine(Path.GetTempPath(), uniqueFileName);

    try
    {
      // TODO: Save file contents to a persistent storage (S3, Azure Blob, etc.) if needed
      await SaveTemporaryFile(request.File, filePath, cancellationToken);
      var jobId = await _importJobRepository.SubmitImportJob(uniqueFileName, cancellationToken);
      await WriteToFilePathQueue(filePath, cancellationToken);

      return ApiResponse<ImportPatientsResponse>.Succeed(new ImportPatientsResponse(jobId));
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error occurred while importing patients.");
      return new ApiResponse<ImportPatientsResponse> { Success = false, Errors = ["An error occurred while processing the import request."] };
    }
  }

  #region Private Methods

  private async Task WriteToFilePathQueue(string filePath, CancellationToken cancellationToken)
  {
    await _filePathQueue.Writer.WriteAsync(filePath, cancellationToken);
  }

  private static async Task SaveTemporaryFile(IFormFile file, string filePath, CancellationToken cancellationToken)
  {
    using var stream = new FileStream(filePath, FileMode.Create);
    await file.CopyToAsync(stream, cancellationToken);
  }

  #endregion
}
