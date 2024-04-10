using System.Text;
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
            var memUsage = "";

            foreach (var metric in item.Usage)
            {
                switch (metric.Key)
                {
                    case "cpu":
                        // Convert form n to m
                        cpuUsage = ConvertToHumanReadable(metric);
                        break;
                    case "memory":
                        // Convert from Ki to Mi
                        memUsage = ConvertToHumanReadable(metric);
                        break;
                }
            }

            table.AddRow(nodeName, cpuUsage, memUsage);
        }

        table.Options.EnableCount = false;
        table.Write();
    }

    public async Task PodMetricsForNamespace(string namespaceName)
    {
        var podMetrics = await kubeClient.GetKubernetesPodsMetricsByNamespaceAsync(namespaceName).ConfigureAwait(false);
        var table = new ConsoleTable("NAMESPACE", "POD NAME", "CONTAINER NAME(S)", "CPU(cores)", "MEMORY(bytes)");
        var pods = podMetrics.Items;
        if (pods is null
            || pods.Any() is false
           )
        {
            return;
        }

        foreach (var pod in pods)
        {
            var podName = pod.Metadata.Name;
            var containerNames = new StringBuilder();
            var cpuUsages = new StringBuilder();
            var memUsages = new StringBuilder();
            foreach (var container in pod.Containers)
            {
                containerNames.Append(container.Name + " ");
                foreach (var metric in container.Usage)
                {
                    switch (metric.Key)
                    {
                        case "cpu":
                            // Convert form n to m
                            cpuUsages.Append(ConvertToHumanReadable(metric) + " ");
                            break;
                        case "memory":
                            // Convert from Ki to Mi
                            memUsages.Append(ConvertToHumanReadable(metric) + " ");
                            break;
                    }
                }
            }

            table.AddRow(
                namespaceName,
                podName,
                containerNames.ToString().Replace(" ", ", ").TrimEnd(',', ' '),
                cpuUsages.ToString().Replace(" ", ", ").TrimEnd(',', ' '),
                memUsages.ToString().Replace(" ", ", ").TrimEnd(',', ' ')
            );
        }

        table.Options.EnableCount = false;
        table.Write();
    }

    private string ConvertToHumanReadable(KeyValuePair<string, ResourceQuantity> metric)
    {
        return metric.Key switch
        {
            "cpu" =>
                // Convert form n to m
                $"{metric.Value * 1024:N0}m",
            "memory" =>
                // Convert from Ki to Mi
                $"{metric.Value / 1024 / 1024:#}Mi",
            _ => "Invalid resource type"
        };
    }
}