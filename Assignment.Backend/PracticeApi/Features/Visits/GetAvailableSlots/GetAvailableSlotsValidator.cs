using FluentValidation;
using PracticeApi.Shared;

namespace PracticeApi.Features.Visits.GetAvailableSlots;

/// <summary>
/// Validator for <see cref="GetAvailableSlotsRequest"/>
/// </summary>
public class GetAvailableSlotsValidator : AbstractValidator<GetAvailableSlotsRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="GetAvailableSlotsValidator"/> class
  /// </summary>
  public GetAvailableSlotsValidator()
  {
    RuleFor(x => x.VisitDate)
      .NotEmpty()
      .WithMessage("Visit date is required.")
      .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
      .WithMessage("Visit date must be today or in the future.");

    RuleFor(x => x.ProviderId)
      .GreaterThan(0)
      .WithMessage("Provider ID must be a positive integer.");
  }
}
