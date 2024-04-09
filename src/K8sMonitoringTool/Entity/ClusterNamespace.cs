using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace K8sMonitoringTool.Entity;

[PrimaryKey("id")]
[Table("k8s_namespaces")]
[Index(nameof(Name), IsUnique = true)] // Add unique to the namespace name
public class ClusterNamespace
{
    public int id { get; }

    [Key]
    public required string Name { get; set; }
    
    public DateTime? CreationTimestamp { get; set; }
    public DateTime? DeletionTimestamp { get; set; }
}