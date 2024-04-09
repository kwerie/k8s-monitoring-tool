using k8s;
using Microsoft.EntityFrameworkCore;

namespace K8sMonitoringTool.Shared;

public class Configuration
{
    // TODO - move this to be fetched with an environment variable
    public readonly KubernetesClientConfiguration KubeClientConfig =
        KubernetesClientConfiguration.BuildConfigFromConfigFile(Path.Combine(Directory.GetCurrentDirectory(),
            ".kube/config"));

    // TOOD - move this to an environment variable as well (concat from multiple environment variables
    public readonly string ConnectionString = "server=localhost;port=3306;database=local;user=local;password=local";

    public readonly MySqlServerVersion MysqlServerVersion = new(new Version(8, 0, 29));
}