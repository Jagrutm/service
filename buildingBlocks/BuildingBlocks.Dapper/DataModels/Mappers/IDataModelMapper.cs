namespace BuildingBlocks.Dapper.Mappers
{
    public interface IDataModelMapper
    {
        TDataModel ToDataModel<TDataModel>(object entity);

        TEntity ToEntity<TEntity>(object dataModel);
    }
}
