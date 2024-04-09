using k8s;
using k8s.Models;
using K8sMonitoringTool.Shared;

namespace K8sMonitoringTool.Service;

public class KubeService(KubeClient client)
{
    public V1NamespaceList ListNameSpaces()
    {
        return client.CoreV1.ListNamespace();
    }

    public V1PodList GetNameSpacePods(V1Namespace ns)
    {
        return client.CoreV1.ListNamespacedPod(ns.Metadata.Name);
    }
}