using K8sMonitoringTool.Entity;
using K8sMonitoringTool.Shared;

namespace K8sMonitoringTool.Repository;

public class ClusterNamespaceRepository(DatabaseContext databaseContext)
    : AbstractRepository<ClusterNamespace>(databaseContext)
{
    public ClusterNamespace? GetByName(string name)
    {
        return DbSet.FirstOrDefault(clusterNamespace => clusterNamespace.Name == name);
    }
}