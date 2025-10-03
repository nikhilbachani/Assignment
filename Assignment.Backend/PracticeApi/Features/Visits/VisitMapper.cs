using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Features.Visits;

/// <summary>
/// Mapper class for <see cref="Visit"/> and <see cref="VisitDto"/>
/// </summary>
public static class VisitMapper
{
  /// <summary>
  /// Maps a Visit entity to a VisitDto
  /// </summary>
  /// <param name="visit"></param>
  /// <returns></returns>
  public static VisitDto ToDto(this Visit visit)
  {
    ArgumentNullException.ThrowIfNull(visit, nameof(visit));

    return new VisitDto
    {
      VisitId = visit.Id,
      VisitDate = visit.VisitDate,
      VisitTime = visit.VisitTime,
      ProviderName = visit.Provider.FirstName + " " + visit.Provider.LastName,
      PatientName = visit.Patient.FirstName + " " + visit.Patient.LastName,
      Notes = visit.Notes,
      PaymentStatus = visit.Invoice?.PaymentStatus.ToString(),
      PaymentMethod = visit.Invoice?.Receipt?.PaymentMethod.ToString(),
      ReceiptIdentifier = visit.Invoice?.Receipt?.ReceiptIdentifier,
      PaymentDate = visit.Invoice?.Receipt?.PaymentDate,
      InvoiceId = visit.Invoice?.Id,
      VisitAmount = visit.Invoice?.Amount,
    };
  }
}
