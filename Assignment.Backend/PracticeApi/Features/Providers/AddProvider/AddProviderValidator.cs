namespace PracticeApi.Features.Providers.AddProvider;

using FluentValidation;

/// <summary>
/// Validator for <see cref="AddProviderRequest"/>
/// </summary>
public class AddProviderValidator : AbstractValidator<AddProviderRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AddProviderValidator"/> class
  /// </summary>
  public AddProviderValidator()
  {
    RuleFor(x => x.FirstName)
      .NotEmpty()
      .WithMessage("First name is required.")
      .MaximumLength(50)
      .WithMessage("First name cannot exceed 50 characters.");

    RuleFor(x => x.LastName)
      .NotEmpty()
      .WithMessage("Last name is required.")
      .MaximumLength(50)
      .WithMessage("Last name cannot exceed 50 characters.");

    RuleFor(x => x.Specialty)
      .NotEmpty()
      .WithMessage("Specialty is required.")
      .MaximumLength(100)
      .WithMessage("Specialty cannot exceed 100 characters.");

    RuleFor(x => x.NpiId)
      .NotEmpty()
      .WithMessage("NPI ID is required.")
      .Matches(ProviderConstants.NpiRegex)
      .WithMessage("NPI ID must be a 10-digit number.");
  }
}
