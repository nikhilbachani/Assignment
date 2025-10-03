namespace PracticeApi.Shared;

/// <summary>
/// Standardized API response wrapper without a body
/// </summary>
public class ApiResponse
{
  /// <summary>
  /// Indicates if the operation was successful
  /// </summary>
  public bool Success { get; set; }

  /// <summary>
  /// List of error messages if not successful
  /// </summary>
  public IEnumerable<string>? Errors { get; set; }

  /// <summary>
  /// Creates a failed API response
  /// </summary>
  /// <param name="errors"></param>
  /// <returns></returns>
  public static ApiResponse Fail(params string[] errors) => new() { Success = false, Errors = errors };
}


/// <summary>
/// Standardized API response wrapper with a body
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T> : ApiResponse
{
  /// <summary>
  /// Response body if successful
  /// </summary>
  public T? Body { get; set; }

  /// <summary>
  /// Creates a successful API response
  /// </summary>
  /// <param name="body"></param>
  /// <returns></returns>
  public static ApiResponse<T> Succeed(T body) => new() { Success = true, Body = body };
}