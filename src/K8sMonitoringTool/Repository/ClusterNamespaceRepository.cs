using K8sMonitoringTool.Entity;
using K8sMonitoringTool.Shared;
using Microsoft.EntityFrameworkCore;

namespace K8sMonitoringTool.Repository;

public class ClusterNamespaceRepository(DatabaseContext databaseContext)
    : AbstractRepository<ClusterNamespace>(databaseContext)
{
    public ClusterNamespace? GetByName(string name)
    {
        return DbSet.FirstOrDefault(clusterNamespace => clusterNamespace.Name == name);
    }

    public List<ClusterNamespace> GetAll()
    {
        return DbSet.ToList();
    }

    public List<ClusterNamespace> GetAllActive()
    {
        return DbSet.Where(ns => ns.DeletionTimestamp == null).ToList();
    }
}