using ConsoleTables;
using k8s;
using k8s.Models;
using K8sMonitoringTool.Shared;

namespace K8sMonitoringTool.Service;

public class MetricsService(KubeClient kubeClient)
{
    public async Task NodesMetrics()
    {
        var nodesMetrics = await kubeClient.GetKubernetesNodesMetricsAsync().ConfigureAwait(false);
        var table = new ConsoleTable("NAME", "CPU(cores)", "MEMORY(bytes)");
        foreach (var item in nodesMetrics.Items)
        {
            var nodeName = item.Metadata.Name;
            var cpuUsage = "";
            var memoryUsage = "";

            foreach (var metric in item.Usage)
            {
                switch (metric.Key)
                {
                    case "cpu":
                        // Convert form n to m
                        cpuUsage = $"{metric.Value * 1024:N0}m";
                        break;
                    case "memory":
                        // Convert from Ki to Mi
                        memoryUsage = $"{metric.Value / 1024 / 1024:#}Mi";
                        break;
                }
            }

            table.AddRow(nodeName, cpuUsage, memoryUsage);
        }

        table.Options.EnableCount = false;
        table.Write();
    }

    public async Task PodMetricsForNamespace(string namespaceName)
    {
        var podMetrics = await kubeClient.GetKubernetesPodsMetricsByNamespaceAsync(namespaceName).ConfigureAwait(false);
        var table = new ConsoleTable("NAMESPACE", "POD NAME", "CONTAINER NAME", "CPU(cores)", "MEMORY(bytes)");
        var pods = podMetrics.Items;
        if (pods is null
            || pods.Any() is false
           )
        {
            return;
        }
        
        // TODO - this does not always show accurate information, sometimes pod names fluctuate
        // could be a possible bug due to pods having multiple replica's

        foreach (var pod in pods)
        {
            var podName = pod.Metadata.Name;
            var containerName = "";
            var cpuUsage = "";
            var memUsage = "";
            // Console.WriteLine($"Pod name: {pod.Metadata.Name}");
            foreach (var container in pod.Containers)
            {
                // Console.WriteLine($"Container name: {container.Name}");
                containerName = container.Name;
                foreach (var metric in container.Usage)
                {
                    switch (metric.Key)
                    {
                        case "cpu":
                            // Convert form n to m
                            // Console.WriteLine($"{metric.Value * 1024:N0}m");
                            cpuUsage = $"{metric.Value * 1024:N0}m";
                            break;
                        case "memory":
                            // Convert from Ki to Mi
                            // Console.WriteLine($"{metric.Value / 1024 / 1024:#}Mi");
                            memUsage = $"{metric.Value / 1024 / 1024:#}Mi";
                            break;
                    }
                }
            }

            table.AddRow(namespaceName, podName, containerName, cpuUsage, memUsage);
        }

        table.Options.EnableCount = false;
        table.Write();
    }
}