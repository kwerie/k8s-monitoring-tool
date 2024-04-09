using System.Linq.Expressions;

namespace K8sMonitoringTool.Repository;

public interface IRepository<TEntity>
{
    
    public TEntity? GetById(int id);

    public IEnumerable<TEntity?> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        IEnumerable<string>? includeProperties = null
    );
}