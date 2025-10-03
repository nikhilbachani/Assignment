using Microsoft.EntityFrameworkCore;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Infrastructure.Data;

/// <summary>
/// Defines the contract for patient repository operations
/// </summary>
public interface IPatientRepository
{
  /// <summary>
  /// Gets a patient by their ID
  /// </summary>
  /// <param name="id"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<Patient?> GetPatientById(int id, CancellationToken cancellationToken = default);

  /// <summary>
  /// Searches for patients by a search string (first name, last name, email)
  /// </summary>
  /// <param name="searchString"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<IReadOnlyCollection<Patient>> SearchPatients(string searchString, CancellationToken cancellationToken = default);

  /// <summary>
  /// Adds a new patient to the repository
  /// </summary>
  /// <param name="patient"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<int> AddPatient(Patient patient, CancellationToken cancellationToken = default);
}

/// <summary>
/// Patient repository implementation for <see cref="IPatientRepository"/>
/// </summary>
/// <param name="logger"></param>
/// <param name="dbContext"></param>
public class PatientRepository(ILogger<PatientRepository> logger, PracticeDbContext dbContext) : IPatientRepository
{
  private readonly ILogger<PatientRepository> _logger = logger;
  private readonly PracticeDbContext _dbContext = dbContext;

  /// <inheritdoc cref="IPatientRepository.GetPatientById"/>
  public async Task<Patient?> GetPatientById(int id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Fetching patient with ID: {Id} from repository.", id);

    var patient = await _dbContext.Patients.FindAsync([id], cancellationToken: cancellationToken);

    if (patient == null)
    {
      _logger.LogWarning("Patient with ID: {Id} not found in repository.", id);
      return null;
    }

    _logger.LogInformation("Patient with ID: {Id} retrieved from repository.", id);
    return patient;
  }

  /// <inheritdoc cref="IPatientRepository.AddPatient"/>
  public async Task<int> AddPatient(Patient patient, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Adding new patient to repository.");

    await _dbContext.Patients.AddAsync(patient, cancellationToken);
    await _dbContext.SaveChangesAsync(cancellationToken);
    
    var id = patient.Id;

    _logger.LogInformation("Patient with ID: {PatientId} added to repository.", id);
    return id;
  }

  /// <inheritdoc cref="IPatientRepository.SearchPatients"/>
  public async Task<IReadOnlyCollection<Patient>> SearchPatients(string searchString, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Searching for patient with search string: {SearchString}", searchString.Trim());

    searchString = searchString.Trim().ToLower();
    
    var results = await _dbContext.Patients
      .Where(p => p.FirstName.ToLower().Contains(searchString) ||
        p.LastName.ToLower().Contains(searchString) ||
        p.Email.ToLower().Contains(searchString) ||
        (p.FirstName + " " + p.LastName).ToLower().Contains(searchString)
      ).ToListAsync(cancellationToken);

    _logger.LogInformation("Found {ResultCount} patients matching search string: {SearchString}", results.Count, searchString);

    return results;
  }
}
