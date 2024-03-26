using AutoMapper;

namespace BuildingBlocks.Dapper.Mappers
{
    public class DataModelMapper : IDataModelMapper
    {
        private readonly IMapper _mapper;

        public DataModelMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDataModel ToDataModel<TDataModel>(object entity)
        {
            return _mapper.Map<TDataModel>(entity);
        }

        public TEntity ToEntity<TEntity>(object dataModel)
        {
            return _mapper.Map<TEntity>(dataModel);
        }
    }
}
