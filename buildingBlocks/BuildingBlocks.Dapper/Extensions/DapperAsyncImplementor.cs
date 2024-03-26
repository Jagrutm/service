using System.Data;
using System.Dynamic;
using BuildingBlocks.Dapper.Extensions.Dialects;
using BuildingBlocks.Dapper.Extensions.Mappers;
using Dapper;

namespace BuildingBlocks.Dapper.Extensions
{
    public interface IDapperAsyncImplementor : IDapperImplementor
    {
        Task<TEntity> GetAsync<TEntity>(IDbConnection connection, dynamic id, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<IEnumerable<TEntity>> GetListAsync<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<IEnumerable<TEntity>> GetPageAsync<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task InsertAsync<TEntity>(IDbConnection connection, IEnumerable<TEntity> entities, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<dynamic> InsertAsync<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<bool> UpdateAsync<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<bool> DeleteAsync<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<bool> DeleteAsync<TEntity>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<IEnumerable<TEntity>> GetSetAsync<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<int> CountAsync<TEntity>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        Task<IMultipleResultReader> GetMultipleAsync(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout);
    }

    public class DapperAsyncImplementor : DapperImplementor, IDapperAsyncImplementor
    {
        public DapperAsyncImplementor(ISqlGenerator sqlGenerator) : base(sqlGenerator) { }

        public async Task<TEntity> GetAsync<TEntity>(IDbConnection connection, dynamic id, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate predicate = GetIdPredicate(classMap, id);
            var data = await GetListAsync<TEntity>(connection, classMap, predicate, null, transaction, commandTimeout);
            TEntity result = data.SingleOrDefault();
            return result;
        }
        public async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return await GetListAsync<TEntity>(connection, classMap, wherePredicate, sort, transaction, commandTimeout);
        }

        public async Task<IEnumerable<TEntity>> GetPageAsync<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return await GetPageAsync<TEntity>(connection, classMap, wherePredicate, sort, page, resultsPerPage, transaction, commandTimeout);
        }

        public async Task InsertAsync<TEntity>(IDbConnection connection, IEnumerable<TEntity> entities, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            string sql = GetInsertQueryUpdateColumnValue<TEntity>(classMap, entities);

            await connection.ExecuteAsync(sql, entities, transaction, commandTimeout, CommandType.Text);
        }

        public async Task<dynamic> InsertAsync<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            List<IPropertyMap> nonIdentityKeyProperties = classMap.Properties
                                                            .Where(p => p.KeyType == KeyType.Guid || p.KeyType == KeyType.Assigned).ToList();

            string sql = GetInsertQueryUpdateColumnValue<TEntity>(classMap, nonIdentityKeyProperties, entity);

            IDictionary<string, object> keyValues = new ExpandoObject();
            var identityColumn = classMap.Properties.SingleOrDefault(p => p.KeyType == KeyType.Identity);

            if (identityColumn != null)
            {
                IEnumerable<long> result;
                if (SqlGenerator.SupportsMultipleStatements())
                {
                    sql += SqlGenerator.Configuration.Dialect.BatchSeperator + SqlGenerator.IdentitySql(classMap);
                    result = await connection.QueryAsync<long>(sql, entity, transaction, commandTimeout, CommandType.Text);
                }
                else
                {
                    await connection.ExecuteAsync(sql, entity, transaction, commandTimeout, CommandType.Text);
                    sql = SqlGenerator.IdentitySql(classMap);
                    result = await connection.QueryAsync<long>(sql, entity, transaction, commandTimeout, CommandType.Text);
                }

                long identityValue = result.First();
                int identityInt = Convert.ToInt32(identityValue);
                keyValues.Add(identityColumn.Name, identityInt);
                identityColumn.PropertyInfo.SetValue(entity, identityInt, null);
            }
            else
            {
                await connection.ExecuteAsync(sql, entity, transaction, commandTimeout, CommandType.Text);
            }

            return GetInsertKeyValues<TEntity>(keyValues, nonIdentityKeyProperties, entity);
        }
        public async Task<bool> UpdateAsync<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            var predicate = GetPredicateForUpdate<TEntity>(classMap, entity);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Update(classMap, predicate, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters = GetDynamicParametersForUpdate<TEntity>(classMap, parameters, entity);

            return await connection.ExecuteAsync(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }

        public async Task<bool> DeleteAsync<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate predicate = GetKeyPredicate<TEntity>(classMap, entity);
            return await DeleteAsync<TEntity>(connection, classMap, predicate, transaction, commandTimeout);
        }

        public async Task<bool> DeleteAsync<TEntity>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return await DeleteAsync<TEntity>(connection, classMap, wherePredicate, transaction, commandTimeout);
        }

        public async Task<IEnumerable<TEntity>> GetSetAsync<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return await GetSetAsync<TEntity>(connection, classMap, wherePredicate, sort, firstResult, maxResults, transaction, commandTimeout);
        }

        public async Task<int> CountAsync<TEntity>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Count(classMap, wherePredicate, parameters);

            var record = await connection.QueryAsync(sql, GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text);

            return (int)record.Single().Total;
        }

        public async Task<IMultipleResultReader> GetMultipleAsync(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            if (SqlGenerator.SupportsMultipleStatements())
            {
                return await GetMultipleByBatchAsync(connection, predicate, transaction, commandTimeout);
            }

            return await GetMultipleBySequenceAsync(connection, predicate, transaction, commandTimeout);
        }

        protected async Task<IEnumerable<TEntity>> GetSetAsync<TEntity>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.SelectSet(classMap, predicate, sort, firstResult, maxResults, parameters);

            return await connection.QueryAsync<TEntity>(sql, GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text);
        }

        protected async Task<GridReaderResultReader> GetMultipleByBatchAsync(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            var sql = GetQueryForMulitpleByBatch(predicate, parameters);

            SqlMapper.GridReader grid = await connection.QueryMultipleAsync(sql.ToString(), GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text);
            return new GridReaderResultReader(grid);
        }

        protected async Task<SequenceReaderResultReader> GetMultipleBySequenceAsync(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            IList<SqlMapper.GridReader> items = new List<SqlMapper.GridReader>();
            foreach (var item in predicate.Items)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                IClassMapper classMap = SqlGenerator.Configuration.GetMap(item.Type);
                IPredicate itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                {
                    itemPredicate = GetPredicate(classMap, item.Value);
                }

                string sql = SqlGenerator.Select(classMap, itemPredicate, item.Sort, parameters);

                SqlMapper.GridReader queryResult = await connection.QueryMultipleAsync(sql, GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text);
                items.Add(queryResult);
            }

            return new SequenceReaderResultReader(items);
        }

        protected async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Select(classMap, predicate, sort, parameters);

            return await connection.QueryAsync<TEntity>(sql, GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text);
        }

        protected async Task<IEnumerable<TEntity>> GetPageAsync<TEntity>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.SelectPaged(classMap, predicate, sort, page, resultsPerPage, parameters);

            return await connection.QueryAsync<TEntity>(sql, GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text);
        }

        protected async Task<bool> DeleteAsync<TEntity>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Delete(classMap, predicate, parameters);

            return await connection.ExecuteAsync(sql, GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text) > 0;
        }
    }
}
