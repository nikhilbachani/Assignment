using FluentValidation;

using PracticeApi.Shared;

namespace PracticeApi.Middleware;

/// <summary>
/// Middleware for handling exceptions globally
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
  private readonly RequestDelegate _next = next;
  private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

  /// <summary>
  /// Invokes the middleware to handle exceptions
  /// </summary>
  /// <param name="context"></param>
  /// <returns></returns>
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (ValidationException ex)
    {
      _logger.LogError(ex, "Validation Error: {Errors}.", ex.Errors);
      var errors = ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
      var response = ApiResponse.Fail([.. errors]);

      var result = Results.BadRequest(response);
      await result.ExecuteAsync(context);
    }
    catch (Exception ex)
    {
      const string genericErrorMessage = "An unexpected error occurred. Please try again later.";
      _logger.LogError(ex, genericErrorMessage);

      var response = ApiResponse.Fail(genericErrorMessage);

      var result = Results.InternalServerError(response);
      await result.ExecuteAsync(context);
    }
  }
}
