namespace K8sMonitoringTool.EntityFactory;

public interface IEntityFactory<TEntity>
{
    public TEntity Create(object data)
    {
        // Method will be implemented with other types of data
        throw new NotImplementedException();
    }

    public TEntity Update(TEntity entity, object data)
    {
        // Method will be implemented with other types of data
        throw new NotImplementedException();
    }
}