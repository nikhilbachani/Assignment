using FluentValidation;
using PracticeApi.Shared;

namespace PracticeApi.Features.Visits.AddVisit;

/// <summary>
/// Validator for adding a new visit
/// </summary>
public class AddVisitValidator : AbstractValidator<AddVisitRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AddVisitValidator"/> class
  /// </summary>
  public AddVisitValidator()
  {
    RuleFor(x => x.PatientId)
      .GreaterThan(0).WithMessage("PatientId must be a positive integer.");

    RuleFor(x => x.ProviderId)
      .GreaterThan(0).WithMessage("ProviderId must be a positive integer.");

    RuleFor(x => x.VisitDate)
      .NotEmpty()
      .WithMessage("VisitDate must be a valid date.")
      .MustBeInFuture()
      .WithMessage("VisitDate must be today or in the future.");

    RuleFor(x => x.VisitTime)
      .NotEmpty()
      .WithMessage("VisitTime is required.")
      .MustBeInFormat(VisitConstants.TimeFormat)
      .WithMessage($"VisitTime must be in {VisitConstants.TimeFormat} format.")
      .MustBeInMinuteIncrements(VisitConstants.SlotDurationMinutes)
      .WithMessage($"VisitTime must be in {VisitConstants.SlotDurationMinutes}-minute increments.")
      .Must(BeWithinBusinessHours)
      .WithMessage("VisitTime must be within business hours (9 AM to 5 PM).");

    RuleFor(x => x.Notes)
      .MaximumLength(500)
      .WithMessage("Notes cannot exceed 500 characters.");
  }

  private static bool BeWithinBusinessHours(TimeOnly time)
  {
    var hour = time.Hour;
    return hour >= VisitConstants.ClinicOpenHour && hour < VisitConstants.ClinicCloseHour;
  }
}
