using FluentValidation;

namespace PracticeApi.Features.Providers.GetProviderById;

/// <summary>
/// Validator for <see cref="GetProviderByIdRequest"/>
/// </summary>
public class GetProviderByIdValidator : AbstractValidator<GetProviderByIdRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="GetProviderByIdValidator"/> class
  /// </summary>
  public GetProviderByIdValidator()
  {
    RuleFor(x => x.Id)
      .GreaterThan(0)
      .WithMessage("Id must be greater than 0");
  }
}