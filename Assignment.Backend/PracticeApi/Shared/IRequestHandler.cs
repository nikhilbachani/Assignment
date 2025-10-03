namespace PracticeApi.Shared;

/// <summary>
/// Generic request handler interface
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IRequestHandler<TRequest, TResponse>
{
  /// <summary>
  /// Handles the incoming request and returns a response
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<ApiResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default);
}