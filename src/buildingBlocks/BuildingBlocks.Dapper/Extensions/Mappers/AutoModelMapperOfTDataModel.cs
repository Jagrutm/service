using System;
using BuildingBlocks.Dapper.DataModels;
using BuildingBlocks.Dapper.Extensions.Mappers;

namespace BuildingBlocks.Persistance.Mapping
{
    public class AutoModelMapper<TDataModel> : ModelMapper<TDataModel, long> where TDataModel : DataModelBase<long>
    {
        public AutoModelMapper()
        {
            Map(_ => _.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }

    public class AutoModelMapperOfGuidPrimaryKey<TDataModel> : ModelMapper<TDataModel, Guid> where TDataModel : DataModelBase<Guid>
    {
        public AutoModelMapperOfGuidPrimaryKey()
        {
            Map(_ => _.Id).Key(KeyType.Guid);
            AutoMap();
        }
    }
}
