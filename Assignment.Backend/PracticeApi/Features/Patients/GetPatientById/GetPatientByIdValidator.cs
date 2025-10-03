using FluentValidation;

namespace PracticeApi.Features.Patients.GetPatientById;

/// <summary>
/// Validator for <see cref="GetPatientByIdRequest"/>
/// </summary>
public class GetPatientByIdValidator : AbstractValidator<GetPatientByIdRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="GetPatientByIdValidator"/> class
  /// </summary>
  public GetPatientByIdValidator()
  {
    RuleFor(x => x.Id)
      .GreaterThan(0)
      .WithMessage("Id must be greater than 0");
  }
}
