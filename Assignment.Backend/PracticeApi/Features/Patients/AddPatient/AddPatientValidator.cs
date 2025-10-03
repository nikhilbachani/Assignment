using FluentValidation;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Features.Patients.AddPatient;

/// <summary>
/// Validator for <see cref="AddPatientRequest"/>
/// </summary>
public class AddPatientValidator : AbstractValidator<AddPatientRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AddPatientValidator"/> class
  /// </summary>
  public AddPatientValidator()
  {
    RuleFor(x => x.FirstName)
      .NotEmpty()
      .WithMessage("First name is required.")
      .MaximumLength(PatientConstants.MaxNameLength)
      .WithMessage($"First name must not exceed {PatientConstants.MaxNameLength} characters.");

    RuleFor(x => x.LastName)
      .NotEmpty()
      .WithMessage("Last name is required.")
      .MaximumLength(PatientConstants.MaxNameLength)
      .WithMessage($"Last name must not exceed {PatientConstants.MaxNameLength} characters.");

    RuleFor(x => x.Email)
      .NotEmpty()
      .WithMessage("Email is required.")
      .EmailAddress()
      .WithMessage("Invalid email format.");

    RuleFor(x => x.DOB)
      .NotEmpty()
      .WithMessage("Date of birth is required.")
      .LessThan(DateOnly.FromDateTime(DateTime.Today)) // DOB must be in the past
      .WithMessage("Date of birth must be in the past.");

    RuleFor(x => x.Phone)
      .NotEmpty()
      .WithMessage("Phone number is required.")
      .Matches(PatientConstants.PhoneRegex)
      .WithMessage("Invalid phone number format.");

    RuleFor(x => x.SSN)
      .NotEmpty()
      .WithMessage("SSN is required.")
      .Matches(PatientConstants.SsnRegex)
      .WithMessage("Invalid SSN format.");
  }
}
