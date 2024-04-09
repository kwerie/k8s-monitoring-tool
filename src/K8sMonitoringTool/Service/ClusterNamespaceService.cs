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
    public ClusterNamespace CreateOrUpdate(V1Namespace ns)
    {
        var existingClusterNamespace = clusterNamespaceRepository.GetByName(ns.Metadata.Name);
        return existingClusterNamespace is not null
            ? clusterNamespaceEntityFactory.Update(existingClusterNamespace, ns)
            : clusterNamespaceEntityFactory.Create(ns);
    }

    public IEnumerable<ClusterNamespace> GetAll(bool includeDeleted = false)
    {
        return !includeDeleted ? clusterNamespaceRepository.GetAllActive() : clusterNamespaceRepository.GetAll();
    }

    public void CheckForDeletedNamespaces(IEnumerable<V1Namespace> fetchedNamespaces)
    {
        // TODO - optimize
        var currentNamespaceNames = clusterNamespaceRepository.GetAll().Select(ns => ns.Name).ToList();
        var fetchedNamespaceNames = fetchedNamespaces.Select(fNs => fNs.Metadata.Name).ToList();
        var difference = currentNamespaceNames.Except(fetchedNamespaceNames).ToList();

        foreach (var diff in difference)
        {
            var clusterNamespace = clusterNamespaceRepository.GetByName(diff);
            if (clusterNamespace is not null)
            {
                clusterNamespaceEntityFactory.Update(clusterNamespace, new V1Namespace
                {
                    Metadata = new V1ObjectMeta
                    {
                        CreationTimestamp =
                            clusterNamespace
                                .CreationTimestamp, // Otherwise the CreationTimestamp will be null in the database.
                        DeletionTimestamp = DateTime.Now
                    }
                });
            }
        }
    }
}