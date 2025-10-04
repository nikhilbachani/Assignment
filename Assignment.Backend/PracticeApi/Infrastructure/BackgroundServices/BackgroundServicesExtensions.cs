using System.Threading.Channels;

namespace PracticeApi.Infrastructure.BackgroundServices;

/// <summary>
/// Extension methods for registering background services
/// </summary>
public static class BackgroundServicesExtensions
{
  /// <summary>
  /// Extension method to add background services to the service collection
  /// </summary>
  /// <param name="services"></param>
  /// <returns></returns>
  public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
  {
    var filePathQueue = Channel.CreateUnbounded<string>();
    services.AddSingleton(filePathQueue);
    services.AddHostedService<CsvImportService>();

    return services;
  }
}