using k8s.Models;
using K8sMonitoringTool.Entity;
using K8sMonitoringTool.EntityFactory;
using K8sMonitoringTool.Repository;

namespace K8sMonitoringTool.Service;

public class ClusterNamespaceService(
    ClusterNamespaceEntityFactory clusterNamespaceEntityFactory,
    ClusterNamespaceRepository clusterNamespaceRepository
    )
{
    public ClusterNamespace createOrUpdate(V1Namespace ns)
    {
        var existingClusterNamespace = clusterNamespaceRepository.GetByName(ns.Metadata.Name);
        if (existingClusterNamespace is not null)
        {
            clusterNamespaceEntityFactory.Update(existingClusterNamespace, ns);
        }
        return clusterNamespaceEntityFactory.Create(ns);
    }
}