using Microsoft.EntityFrameworkCore;
using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Infrastructure.Data;

/// <summary>
/// Defines the contract for invoice repository operations
/// </summary>
public interface IInvoiceRepository
{
  /// <summary>
  /// Gets an invoice by ID
  /// </summary>
  /// <param name="id"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<Invoice?> GetInvoiceById(int id, CancellationToken cancellationToken = default);

  /// <summary>
  /// Marks an invoice as paid
  /// </summary>
  /// <param name="id"></param>
  /// <param name="paymentMethod"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  Task<string> PayInvoice(int id, string paymentMethod, CancellationToken cancellationToken = default);
}

/// <summary>
/// Invoice repository implementation for managing invoices
/// </summary>
public class InvoiceRepository(ILogger<InvoiceRepository> logger, PracticeDbContext dbContext) : IInvoiceRepository
{
  private readonly ILogger<InvoiceRepository> _logger = logger;
  private readonly PracticeDbContext _dbContext = dbContext;

  /// <inheritdoc cref="IInvoiceRepository.GetInvoiceById"/>
  public async Task<Invoice?> GetInvoiceById(int id, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Fetching invoice with ID: {Id} from repository.", id);
    var query = _dbContext.Invoices.AsQueryable();
    query = query.Include(i => i.Receipt); // Include receipt details

    return await query.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
  }

  /// <summary>
  /// Marks an invoice as paid
  /// </summary>
  /// <param name="id"></param>
  /// <param name="paymentMethod"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public async Task<string> PayInvoice(int id, string paymentMethod, CancellationToken cancellationToken = default)
  {
    _logger.LogDebug("Attempting to pay invoice with ID: {Id}.", id);

    var invoice = await _dbContext.Invoices
        .Include(i => i.Receipt)
        .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    if (invoice == null)
    {
      _logger.LogWarning("Invoice with ID: {Id} not found.", id);
      return string.Empty;
    }

    if (invoice.PaymentStatus == PaymentStatus.Completed)
    {
      _logger.LogWarning("Invoice with ID: {Id} is already paid.", id);
      return invoice.Receipt?.ReceiptIdentifier!;
    }

    using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

    try
    {
      var newReceipt = new Receipt
      {
        InvoiceId = invoice.Id,
        ReceiptIdentifier = Guid.NewGuid().ToString(),
        Amount = invoice.Amount,
        PaymentDate = DateOnly.FromDateTime(DateTime.Now),
        PaymentMethod = Enum.Parse<PaymentMode>(paymentMethod, true),
      };

      _dbContext.Receipts.Add(newReceipt);
      await _dbContext.SaveChangesAsync(cancellationToken);

      invoice.Receipt = newReceipt;
      invoice.PaymentStatus = PaymentStatus.Completed;

      _dbContext.Invoices.Update(invoice);
      await _dbContext.SaveChangesAsync(cancellationToken);
      await transaction.CommitAsync(cancellationToken);

      return newReceipt.ReceiptIdentifier;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error occurred while paying invoice with ID: {Id}.", id);
      await transaction.RollbackAsync(cancellationToken);
      
      throw;
    }
  }
}