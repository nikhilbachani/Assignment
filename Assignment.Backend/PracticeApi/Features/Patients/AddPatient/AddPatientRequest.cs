namespace PracticeApi.Features.Patients.AddPatient;

/// <summary>
/// Request model for adding a new patient
/// </summary>
public record AddPatientRequest(string FirstName, string LastName, DateOnly DOB, string Email, string Phone, string SSN);