using k8s;

namespace K8sMonitoringTool.Shared;

public class Configuration
{
    // TODO - move this to be fetched with an environment variable
    public readonly KubernetesClientConfiguration KubeClientConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile(Path.Combine(Directory.GetCurrentDirectory(), ".kube/config"));
}