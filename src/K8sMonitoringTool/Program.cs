using k8s;
using K8sMonitoringTool.Service;
using K8sMonitoringTool.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace K8sMonitoringTool;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .UseDefaultServiceProvider((_, options) =>
            {
                options.ValidateScopes = true;
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<Configuration>();
                services.AddSingleton<KubeClient>();
                services.AddTransient<KubeService>();
            })
            .Build();
        var kubeClient = builder.Services.GetRequiredService<KubeService>();

        
        foreach (var ns in kubeClient.ListNameSpaces())
        {
           Console.WriteLine($"Namespace: {ns.Metadata.Name}");
           var pods = kubeClient.GetNameSpacePods(ns);
           if (pods.Items.Count == 0)
           {
               Console.WriteLine("\t Namespace does not contain pods");
           }
           else
           {
               foreach (var pod in pods)
               {
                   Console.WriteLine($"\t Pod name: {pod.Metadata.Name}");
               }
           }
        }
    }
}