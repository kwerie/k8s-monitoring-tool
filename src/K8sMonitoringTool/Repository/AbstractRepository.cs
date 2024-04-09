using System.Linq.Expressions;
using K8sMonitoringTool.Shared;
using Microsoft.EntityFrameworkCore;

namespace K8sMonitoringTool.Repository;

public abstract class AbstractRepository<TEntity>(DatabaseContext databaseContext) : IRepository<TEntity>
    where TEntity : class
{
    protected readonly DbSet<TEntity> DbSet = databaseContext.Set<TEntity>();

    public TEntity? GetById(int id)
    {
        return DbSet.Find(id);
    }

    public IEnumerable<TEntity?> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>>? orderBy = null,
        IEnumerable<string>? includeProperties = null
    )
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var property in includeProperties)
            {
                query.Include(property);
            }
        }

        return orderBy is not null ? orderBy(query).ToList() : query.ToList();
    }
}