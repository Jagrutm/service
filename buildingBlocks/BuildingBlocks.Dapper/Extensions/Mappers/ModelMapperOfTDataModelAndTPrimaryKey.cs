using BuildingBlocks.Dapper.DataModels;
using BuildingBlocks.Dapper.Extensions.Mappers;

namespace BuildingBlocks.Persistance.Mapping
{
    public class ModelMapper<TDataModel,TPrimaryKey> : PluralizedClassMapper<TDataModel>
        where TDataModel : DataModelBase<TPrimaryKey>
    {
        public ModelMapper()
        {
            Type type = typeof(TDataModel);
            Table(type.Name);
        }

        public override void Table(string tableName)
        {
            base.Table(tableName.Replace("DataModel", ""));
        }
    }
}
