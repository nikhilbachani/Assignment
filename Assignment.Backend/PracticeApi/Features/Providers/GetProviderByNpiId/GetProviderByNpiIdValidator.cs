using FluentValidation;

namespace PracticeApi.Features.Providers.GetProviderByNpiId;

/// <summary>
/// Validator for <see cref="GetProviderByNpiIdRequest"/>
/// </summary>
public class GetProviderByNpiIdValidator : AbstractValidator<GetProviderByNpiIdRequest>
{

  /// <summary>
  /// Initializes a new instance of the <see cref="GetProviderByNpiIdValidator"/> class
  /// </summary>
  public GetProviderByNpiIdValidator()
  {
    RuleFor(x => x.NpiId)
      .NotEmpty()
      .WithMessage("NPI ID is required.")
      .Matches(ProviderConstants.NpiRegex)
      .WithMessage("NPI ID must be a 10-digit number.");
  }
}
