using Microsoft.EntityFrameworkCore;
using PracticeApi.Features.Invoices;
using PracticeApi.Features.Visits;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Infrastructure.Data;

/// <summary>
/// Defines the contract for visit repository operations
/// </summary>
public interface IVisitRepository
{
  /// <summary>
  /// Get all visits for a date and/or provider
  /// </summary>
  /// <param name="providerId"></param>
  /// <param name="includeInvoice"></param>
  /// <param name="date"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<IReadOnlyCollection<Visit>> GetVisitsByDate(DateOnly date, int? providerId, bool? includeInvoice, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets a visit by ID
  /// </summary>
  /// <param name="id"></param>
  /// <param name="includeInvoice"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<Visit?> GetVisitById(int id, bool? includeInvoice, CancellationToken cancellationToken = default);

  /// <summary>
  /// Gets available slots for a provider on a specific date
  /// </summary>
  /// <param name="providerId"></param>
  /// <param name="date"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<List<TimeOnly>> GetAvailableSlots(int providerId, DateOnly date, CancellationToken cancellationToken = default);

  /// <summary>
  /// Adds a new visit
  /// </summary>
  /// <param name="visit"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<int> AddVisit(Visit visit, CancellationToken cancellationToken = default);
}

/// <summary>
/// Visit repository implementation for managing visits
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="VisitRepository"/> class
/// </remarks>
/// <param name="logger"></param>
/// <param name="dbContext"></param>
public class VisitRepository(ILogger<VisitRepository> logger, PracticeDbContext dbContext) : IVisitRepository
{
  private readonly ILogger<VisitRepository> _logger = logger;
  private readonly PracticeDbContext _dbContext = dbContext;

  /// <inheritdoc cref="IVisitRepository.GetVisitsByDate"/>
  public async Task<IReadOnlyCollection<Visit>> GetVisitsByDate(DateOnly visitDate, int? providerId, bool? includeInvoice, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Fetching visits from repository.");

    IQueryable<Visit> query = _dbContext.Visits
      .Where(v => v.VisitDate == visitDate)
      .Include(v => v.Patient) // Include patient details
      .Include(v => v.Provider); // Include provider details

    if (includeInvoice.HasValue && includeInvoice.Value)
    {
      query = query.Include(v => v.Invoice); // Include invoice details
      query = query.Include(v => v.Invoice.Receipt); // Include receipt details if invoice is included
    }

    if (providerId.HasValue)
    {
      query = query.Where(v => v.ProviderId == providerId);
    }

    return await query.ToListAsync(cancellationToken);
  }

  /// <inheritdoc cref="IVisitRepository.GetVisitById"/>
  public async Task<Visit?> GetVisitById(int id, bool? includeInvoice, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Fetching visit with ID: {VisitId} from repository.", id);

    IQueryable<Visit> query = _dbContext.Visits.AsQueryable()
      .Include(v => v.Patient) // Include patient details
      .Include(v => v.Provider); // Include provider details

    if (includeInvoice.HasValue && includeInvoice.Value)
    {
      query = query.Include(v => v.Invoice); // Include invoice details
      query = query.Include(v => v.Invoice.Receipt); // Include receipt details if invoice is included
    }

    return await query.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
  }

  /// <inheritdoc cref="IVisitRepository.AddVisit"/>
  public async Task<int> AddVisit(Visit visit, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Trying to book visit slot: {Date} Time: {Time} with provider {ProviderId}.", visit.VisitDate.ToShortDateString(), visit.VisitTime.ToShortTimeString(), visit.ProviderId);

    var availableSlots = await GetAvailableSlots(visit.ProviderId, visit.VisitDate, cancellationToken);

    if (!availableSlots.Contains(visit.VisitTime))
    {
      _logger.LogWarning("Visit for slot: {TimeSlot} cannot be scheduled with provider: {ProviderId} since the slot is booked.", visit.VisitTime, visit.ProviderId);

      throw new InvalidOperationException("The visit slot is already booked.");
    }

    using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

    try
    {
      // Save the Visit first
      var newVisit = await _dbContext.Visits.AddAsync(visit, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);

      // Create and associate the Invoice with the Visit
      var newInvoice = new Invoice
      {
        VisitId = newVisit.Entity.Id, // Associate the Invoice with the Visit
        Amount = VisitConstants.StandardVisitFee,
        InvoiceDate = DateOnly.FromDateTime(DateTime.Now),
        PaymentStatus = PaymentStatus.Pending
      };

      await _dbContext.Invoices.AddAsync(newInvoice, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);

      await transaction.CommitAsync(cancellationToken);

      return newVisit.Entity.Id;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error occurred while adding visit: {ErrorMessage}", ex.Message);

      await transaction.RollbackAsync(cancellationToken);
      throw;
    }
  }

  /// <inheritdoc cref="IVisitRepository.GetAvailableSlots"/>
  public async Task<List<TimeOnly>> GetAvailableSlots(int providerId, DateOnly date, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Fetching available slots for provider {ProviderId} on {Date}.", providerId, date);

    var allSlots = GetAllTimeSlots(date);
    var availableSlots = allSlots.Except(await GetBookedTimeSlots(providerId, date)).ToList();

    _logger.LogInformation("Available slots for provider {ProviderId} on {Date}: {AvailableSlots}.", providerId, date, availableSlots);

    return availableSlots;
  }

  #region Private Methods

  private static List<TimeOnly> GetAllTimeSlots(DateOnly date)
  {
    var slots = new List<TimeOnly>();
    var startTime = new TimeOnly(VisitConstants.ClinicOpenHour, 0);
    var endTime = new TimeOnly(VisitConstants.ClinicCloseHour, 0).AddMinutes(-VisitConstants.SlotDurationMinutes);

    for (var time = startTime; time <= endTime; time = time.AddMinutes(VisitConstants.SlotDurationMinutes))
    {
      slots.Add(time);
    }

    return slots;
  }

  private async Task<List<TimeOnly>> GetBookedTimeSlots(int providerId, DateOnly date)
  {
    var bookedSlots = await _dbContext.Visits
        .Where(v => v.ProviderId == providerId && v.VisitDate == date)
        .Select(v => v.VisitTime)
        .ToListAsync();

    return bookedSlots;
  }

  #endregion
}
