using FluentValidation;
using PracticeApi.Infrastructure.Data;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Features.Invoices.PayInvoice;

/// <summary>
/// Validator for <see cref="PayInvoiceRequest"/>
/// </summary>
public class PayInvoiceValidator : AbstractValidator<PayInvoiceRequest>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PayInvoiceValidator"/> class.
  /// </summary>
  public PayInvoiceValidator()
  {
    RuleFor(x => x.InvoiceId)
      .GreaterThan(0)
      .WithMessage("InvoiceId must be greater than zero.");

    RuleFor(x => Enum.Parse<PaymentMode>(x.PaymentMethod, true))
      .IsInEnum()
      .WithMessage("PaymentMethod must be either Cash or Card.");
  }
}