using FluentValidation;

namespace PracticeApi.Features.Visits.GetVisitById;

/// <summary>
/// Validator for <see cref="GetVisitByIdRequest"/>
/// </summary>
public class GetVisitByIdValidator : AbstractValidator<GetVisitByIdRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="GetVisitByIdValidator"/> class
  /// </summary>
  public GetVisitByIdValidator()
  {
    RuleFor(x => x.Id)
      .GreaterThan(0)
      .WithMessage("Id must be greater than 0.");
  }
}