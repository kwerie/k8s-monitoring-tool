using k8s;
using K8sMonitoringTool.EntityFactory;
using K8sMonitoringTool.Repository;
using K8sMonitoringTool.Service;
using K8sMonitoringTool.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Timer = System.Timers.Timer;

namespace K8sMonitoringTool;

internal class Program
{
    public static Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .UseDefaultServiceProvider((_, options) => { options.ValidateScopes = true; })
            .ConfigureServices((_, services) =>
            {
                // Singletons
                services.AddSingleton<Configuration>();
                services.AddSingleton<DatabaseContext>();
                services.AddSingleton<KubeClient>();
                services.AddSingleton<ClusterNamespaceEntityFactory>();

                // DbContext
                services.AddDbContext<DatabaseContext>();

                // Transients
                services.AddTransient<KubeService>();
                services.AddTransient<ClusterNamespaceRepository>();
                services.AddTransient<ClusterNamespaceEntityFactory>();
                services.AddTransient<ClusterNamespaceService>();
                services.AddTransient<MetricsService>();
            })
            .Build();
        var clusterNamespaceService = builder.Services.GetRequiredService<ClusterNamespaceService>();
        var kubeService = builder.Services.GetRequiredService<KubeService>();

        var fetchedNamespaces = kubeService.ListNameSpaces().Items;

        foreach (var v1Namespace in fetchedNamespaces)
        {
            clusterNamespaceService.CreateOrUpdate(v1Namespace);
        }

        clusterNamespaceService.CheckForDeletedNamespaces(fetchedNamespaces);

        var namespaces = clusterNamespaceService.GetAll();
        var metricsService = builder.Services.GetRequiredService<MetricsService>();
        var timer = new Timer(1000);

        // Fetch cluster data every second
        timer.Elapsed += async (_, _) =>
        {
            Console.SetCursorPosition(0, 0); // Reset the cursor position, so the data won't constantly blink
            await metricsService.NodesMetrics();

            foreach (var ns in namespaces)
            {
                await metricsService.PodMetricsForNamespace(ns.Name);
            }

            Console.Write("Press any key to exit...");
        };

        timer.Start();
        Console.Read();
        return Task.CompletedTask;
    }
}