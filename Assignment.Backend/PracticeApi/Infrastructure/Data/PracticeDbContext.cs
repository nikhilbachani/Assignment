using Microsoft.EntityFrameworkCore;

using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Infrastructure.Data;

/// <summary>
/// Database context for the practice API
/// </summary>
/// <param name="options"></param>
public class PracticeDbContext(DbContextOptions<PracticeDbContext> options) : DbContext(options)
{
  /// <summary>
  /// Gets or sets the Providers DbSet
  /// </summary>
  public DbSet<Provider> Providers { get; set; } = null!;

  /// <summary>
  /// Gets or sets the Patients DbSet
  /// </summary>
  public DbSet<Patient> Patients { get; set; } = null!;

  /// <summary>
  /// Gets or sets the Visits DbSet
  /// </summary>
  public DbSet<Visit> Visits { get; set; } = null!;

  /// <summary>
  /// Gets or sets the Invoices DbSet
  /// </summary>
  public DbSet<Invoice> Invoices { get; set; } = null!;

  /// <summary>
  /// Gets or sets the Receipts DbSet
  /// </summary>
  public DbSet<Receipt> Receipts { get; set; } = null!;

  /// <inheritdoc cref="DbContext.OnModelCreating"/>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Provider>(entity =>
    {
      entity.HasIndex(e => e.NpiId)
        .IsUnique(); // Ensures Provider NPI ID is unique
    });

    modelBuilder.Entity<Patient>(entity =>
    {
      entity.HasIndex(e => e.SSN)
        .IsUnique(); // Ensures Patient SSN is unique
    });

    modelBuilder.Entity<Visit>(entity =>
    {
      entity.HasIndex(e => new { e.ProviderId, e.VisitDate, e.VisitTime })
        .IsUnique(); // Ensures a provider cannot have overlapping visits
    });

    modelBuilder.Entity<Visit>(entity =>
    {
      entity.HasIndex(e => new { e.PatientId, e.VisitDate, e.VisitTime })
        .IsUnique(); // Ensures a patient cannot have overlapping visits
    });

    modelBuilder.Entity<Patient>()
        .HasIndex(p => p.FirstName)
        .HasDatabaseName("IX_Patient_FirstName");

    modelBuilder.Entity<Patient>()
        .HasIndex(p => p.LastName)
        .HasDatabaseName("IX_Patient_LastName");

    modelBuilder.Entity<Patient>()
        .HasIndex(p => p.Email)
        .HasDatabaseName("IX_Patient_Email");

    modelBuilder.Entity<Visit>(entity =>
    {
      entity.HasIndex(e => new { e.ProviderId, e.VisitDate, e.VisitTime })
        .IsUnique(); // Ensures a provider cannot have overlapping visits
    });

    // Configure Visit -> Patient relationship
    modelBuilder.Entity<Visit>()
      .HasOne(v => v.Patient)
      .WithMany()
      .HasForeignKey(v => v.PatientId)
      .OnDelete(DeleteBehavior.Cascade);

    // Configure Visit -> Provider relationship
    modelBuilder.Entity<Visit>()
      .HasOne(v => v.Provider)
      .WithMany()
      .HasForeignKey(v => v.ProviderId)
      .OnDelete(DeleteBehavior.Restrict);

    // Configure Visit -> Invoice relationship
    modelBuilder.Entity<Visit>()
      .HasOne(v => v.Invoice)
      .WithOne()
      .HasForeignKey<Invoice>(i => i.VisitId)
      .OnDelete(DeleteBehavior.Cascade);

    // Configure Invoice -> Receipt relationship
    modelBuilder.Entity<Invoice>()
      .HasOne(v => v.Receipt)
      .WithOne()
      .HasForeignKey<Receipt>(i => i.InvoiceId)
      .OnDelete(DeleteBehavior.Cascade)
      .IsRequired(false); // Zero or one relationship
  }
}
