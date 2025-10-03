using FluentValidation;

namespace PracticeApi.Features.Invoices.GetInvoiceById;

/// <summary>
/// Validator for <see cref="GetInvoiceByIdRequest"/>
/// </summary>
public class GetInvoiceByIdValidator : AbstractValidator<GetInvoiceByIdRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="GetInvoiceByIdValidator"/> class
  /// </summary>
  public GetInvoiceByIdValidator()
  {
    RuleFor(x => x.Id)
      .GreaterThan(0)
      .WithMessage("Id must be greater than 0.");
  }
}