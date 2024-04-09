using k8s.Models;
using K8sMonitoringTool.Entity;
using K8sMonitoringTool.Shared;
using Microsoft.EntityFrameworkCore;

namespace K8sMonitoringTool.EntityFactory;

public class ClusterNamespaceEntityFactory(DatabaseContext databaseContext): IEntityFactory<ClusterNamespace>
{
    public ClusterNamespace Create(V1Namespace nameSpace)
    {
        var existingNamespace = databaseContext.ClusterNamespaces.FirstOrDefault(ns => ns.Name == nameSpace.Metadata.Name);
        if (existingNamespace is not null)
        {
            return Update(existingNamespace, nameSpace);
        }
        
        var ns = new ClusterNamespace
        {
            Name = nameSpace.Metadata.Name,
            CreationTimestamp = nameSpace.Metadata.CreationTimestamp
        };
        databaseContext.ClusterNamespaces.Add(ns);
        try
        {
            databaseContext.SaveChanges();
        }
        catch (DbUpdateConcurrencyException e)
        {
            Console.WriteLine($"Something went wrong when creating K8sNamespace. \nA possible cause could be: data in the database has been modified since it was loaded into memory. \nException message: {e.Message}");
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Something went wrong when creating K8sNamespace. \nException message: {e.Message}");
        }

        return ns;
    }
    public ClusterNamespace Update(ClusterNamespace existingClusterNamespace, V1Namespace ns)
    {
        // For now only update the deletion timestamp 
        var metadata = ns.Metadata;
        existingClusterNamespace.CreationTimestamp = metadata.CreationTimestamp;
        existingClusterNamespace.DeletionTimestamp = metadata.DeletionTimestamp;
        databaseContext.SaveChanges();
        return existingClusterNamespace;
    }
}
