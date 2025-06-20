using ChangeLogger.Action;
using ChangeLogger.Action.Features.UpdateChangeLog;
using ChangeLogger.Action.Shared;
using ChangeLogger.Action.Shared.Configuration;
using ChangeLogger.Action.Shared.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = CreateHostBuilder()
    .Build();

using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var exit = services.GetRequiredService<App>()
        .Run(args);
    Environment.Exit(exit);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Environment.Exit(1);
}

return;

IHostBuilder CreateHostBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, sc) =>
        {
            sc.AddSingleton<App>();
            sc.AddSingleton<UpdateChangeLogCommand>();
            sc.AddSingleton<IFileReader, FileReader>();
            sc.AddSingleton<IChangeLogTransformer, ChangeLogTransformer>();
            sc.AddSingleton<IGitHubSettings, GitHubSettings>();
            sc.AddSingleton<IInputGenerator, InputGenerator>();
            sc.AddSingleton<IGitClient, GitClient>();
        });
}