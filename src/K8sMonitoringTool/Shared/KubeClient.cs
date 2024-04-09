using k8s;

namespace K8sMonitoringTool.Shared;

public class KubeClient(Configuration configuration) : Kubernetes(configuration.KubeClientConfig);