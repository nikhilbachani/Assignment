using FluentValidation;

namespace PracticeApi.Features.Patients.SearchPatient;

/// <summary>
/// Validator for <see cref="SearchPatientRequest"/>
/// </summary>
public class SearchPatientValidator : AbstractValidator<SearchPatientRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SearchPatientValidator"/> class
  /// </summary>
  public SearchPatientValidator()
  {
    RuleFor(x => x.SearchTerm)
      .NotEmpty()
      .WithMessage("Search term cannot be empty.")
      .MinimumLength(3)
      .WithMessage("Search term must be at least 3 characters long.")
      .MaximumLength(100)
      .WithMessage("Search term cannot exceed 100 characters.")
      .Matches("^[a-zA-Z0-9.\\s]+$")
      .WithMessage("Search term can only contain alphanumeric characters and periods.");
  }
}