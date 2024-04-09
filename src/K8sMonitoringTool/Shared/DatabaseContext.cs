using K8sMonitoringTool.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace K8sMonitoringTool.Shared;

public class DatabaseContext(Configuration configuration) : DbContext
{
    public DbSet<ClusterNamespace> ClusterNamespaces { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseMySql(configuration.ConnectionString, configuration.MysqlServerVersion)
            .LogTo(Console.WriteLine, LogLevel.Warning)
            .EnableDetailedErrors();
    }
}