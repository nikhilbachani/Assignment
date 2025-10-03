using System.Data;
using FluentValidation;
using PracticeApi.Shared;

namespace PracticeApi.Features.Visits.GetVisitsByDate;

/// <summary>
/// Validator for <see cref="GetVisitsByDateRequest"/>
/// </summary>
public class GetVisitsByDateValidator : AbstractValidator<GetVisitsByDateRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="GetVisitsByDateValidator"/> class
  /// </summary>
  public GetVisitsByDateValidator()
  {
    RuleFor(x => x.VisitDate)
      .NotEmpty()
      .WithMessage("Visit date is required.");

    RuleFor(x => x.ProviderId)
      .GreaterThan(0)
      .When(x => x.ProviderId.HasValue)
      .WithMessage("ProviderId must be greater than 0.");
  }
}
