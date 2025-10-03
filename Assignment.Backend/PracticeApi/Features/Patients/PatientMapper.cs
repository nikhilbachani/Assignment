using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Features.Patients;

/// <summary>
/// Mapper class for <see cref="Patient"/> and <see cref="PatientDto"/>
/// </summary>
public static class PatientMapper
{
  /// <summary>
  /// Maps an <see cref="Patient"/> entity to an <see cref="PatientDto"/>
  /// </summary>
  /// <param name="patient"></param>
  /// <returns></returns>
  public static PatientDto ToDto(this Patient patient)
  {
    ArgumentNullException.ThrowIfNull(patient, nameof(patient));

    return new PatientDto
    {
      PatientId = patient.Id,
      PatientName = patient.FirstName + " " + patient.LastName,
      DOB = patient.DOB,
      Email = patient.Email,
      Phone = patient.Phone,
    };
  }
}
