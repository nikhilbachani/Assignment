using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using PracticeApi.Infrastructure.Data.Models;

namespace PracticeApi.Infrastructure.Data;

/// <summary>
/// Extension method related to Practice database
/// </summary>
public static class PracticeDbExtensions
{
  private static SqliteConnection? _connection;

  /// <summary>
  /// Registers <seealso cref="PracticeDbContext"/> in DI container
  /// </summary>
  /// <param name="services"></param>
  public static void AddPracticeDb(this IServiceCollection services)
  {
    // Keep a single open connection for in-memory SQLite
    _connection = new SqliteConnection("Data Source=:memory:");
    _connection.Open();

    services.AddDbContext<PracticeDbContext>(options => options.UseSqlite(_connection));
  }

  /// <summary>
  /// Ensures database and tables are created with mock data
  /// </summary>
  /// <param name="app"></param>
  public static void EnsureDbCreated(this IApplicationBuilder app)
  {
    using var scope = app.ApplicationServices.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<PracticeDbContext>();
    db.Database.EnsureCreated();

    if (!db.Providers.Any())
    {
      db.Providers.AddRange(
          new Provider { FirstName = "Dr.", LastName = "Alice", Specialty = "Cardiology", NpiId = "1234567890" },
          new Provider { FirstName = "Dr.", LastName = "Bob", Specialty = "Dermatology", NpiId = "0987654321" },
          new Provider { FirstName = "Dr.", LastName = "Carol", Specialty = "Pediatrics", NpiId = "1122334455" }
      );

      db.SaveChanges();
    }
  }

  /// <summary>
  /// Registers all repositories in the DI container as scoped.
  /// </summary>
  /// <param name="services">The service collection.</param>
  public static void AddDbRepositories(this IServiceCollection services)
  {
      services.AddScoped<IInvoiceRepository, InvoiceRepository>();
      services.AddScoped<IPatientRepository, PatientRepository>();
      services.AddScoped<IProviderRepository, ProviderRepository>();
      services.AddScoped<IVisitRepository, VisitRepository>();
  }
}
