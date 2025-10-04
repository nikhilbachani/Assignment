using FluentValidation;

namespace PracticeApi.Features.Patients.ImportPatients;

/// <summary>
/// Validator for <see cref="ImportPatientsRequest"/>
/// </summary>
public class ImportPatientsValidator : AbstractValidator<ImportPatientsRequest>
{
  /// <summary>
  /// Constructor for <see cref="ImportPatientsValidator"/>
  /// </summary>
  public ImportPatientsValidator()
  {
    RuleFor(x => x.File)
      .NotNull()
      .WithMessage("File is required.")
      .Must(file => file != null && file.Length > 0)
      .WithMessage("File must not be empty.")
      .Must(file => file != null && file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
      .WithMessage("Only CSV files are allowed.");
  }
}
