using Alderley.DiskManagement.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddScoped<IDiskImportService<string>, DiskImportByConsoleTextService>()
            .AddScoped<IDiskManagementCalculator, DiskManagementCalculator>()
            ;
    })
    .Build();

await ExemplifyCleanUpAsync(host.Services);

await host.RunAsync();

static async Task ExemplifyCleanUpAsync(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    Console.WriteLine("Hello, World! Reading from input.txt...");
    var importService = provider.GetRequiredService<IDiskImportService<string>>();
    var text = File.ReadAllText("input.txt");
    var root = await importService.ImportAsync(text);

    Console.WriteLine("Estimating feasible directory to clean up with at most of 100000...");
    var calculator = provider.GetRequiredService<IDiskManagementCalculator>();
    var directories = calculator.EstimateCleanUp(root);
    Console.WriteLine("These can be cleaned up:");
    foreach (var dir in directories)
    {
        Console.WriteLine($"\t- {dir.Name} with total size of {calculator.SizeOf(dir)}");
    }
}
