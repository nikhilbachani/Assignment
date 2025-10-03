using FluentValidation;

namespace PracticeApi.Shared;

/// <summary>
/// Provides custom validators for FluentValidation.
/// </summary>
public static partial class CustomValidators
{
  /// <summary>
  /// Validates that a DateTime value is in the future.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The rule builder with the custom validator applied.</returns>
  public static IRuleBuilderOptions<T, DateOnly> MustBeInFuture<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
  {
    return ruleBuilder.Must(date => date >= DateOnly.FromDateTime(DateTime.Now));
  }

  /// <summary>
  /// Validates that a TimeOnly value is in standard hour increments.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="increment"></param>
  /// <returns>The rule builder with the custom validator applied.</returns>
  public static IRuleBuilderOptions<T, TimeOnly> MustBeInMinuteIncrements<T>(this IRuleBuilder<T, TimeOnly> ruleBuilder, int increment)
  {
    return ruleBuilder.Must(time => time.Minute % increment == 0);
  }

  /// <summary>
  /// Validates that a <see cref="TimeOnly"/> value is in a specific format.
  /// </summary>
  /// <typeparam name="T">The type of the object being validated.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="format"></param>
  /// <returns>The rule builder with the custom validator applied.</returns>
  public static IRuleBuilderOptions<T, TimeOnly> MustBeInFormat<T>(this IRuleBuilder<T, TimeOnly> ruleBuilder, string format = "HH:mm")
  {
    return ruleBuilder.Must(value => TimeOnly.TryParseExact(value.ToString(), format, out _));
  }
}
