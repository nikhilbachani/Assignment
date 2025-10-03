using FluentValidation;

namespace PracticeApi.Shared;

/// <summary>
/// Base request handler class that provides validation and request handling
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <param name="validator"></param>
public abstract class BaseRequestHandler<TRequest, TResponse>(IValidator<TRequest>? validator = null) : IRequestHandler<TRequest, TResponse>
{
  private readonly IValidator<TRequest>? _validator = validator;

  /// <summary>
  /// Handles the incoming request with validation
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="ValidationException"></exception>
  public async Task<ApiResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default)
  {
    if (_validator != null)
    {
      var result = await _validator.ValidateAsync(request, cancellationToken);

      if (!result.IsValid)
      {
        throw new ValidationException(result.Errors);
      }
    }

    return await HandleRequest(request, cancellationToken);
  }

  /// <summary>
  /// Handles the actual request logic
  /// </summary>
  /// <param name="request"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  protected abstract Task<ApiResponse<TResponse>> HandleRequest(TRequest request, CancellationToken cancellationToken);
}
