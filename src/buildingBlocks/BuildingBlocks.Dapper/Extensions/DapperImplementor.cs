using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BuildingBlocks.Dapper.Extensions.Mappers;
using BuildingBlocks.Dapper.Extensions.Dialects;

namespace BuildingBlocks.Dapper.Extensions
{
    public interface IDapperImplementor
    {
        ISqlGenerator SqlGenerator { get; }
        TEntity Get<TEntity>(IDbConnection connection, dynamic id, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        void Insert<TEntity>(IDbConnection connection, IEnumerable<TEntity> entities, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        dynamic Insert<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        bool Update<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        bool Delete<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        bool Delete<TEntity>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        IEnumerable<TEntity> GetList<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class;
        IEnumerable<TEntity> GetPage<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class;
        IEnumerable<TEntity> GetSet<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class;
        int Count<TEntity>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class;
        IMultipleResultReader GetMultiple(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout);
    }

    public class DapperImplementor : IDapperImplementor
    {
        public DapperImplementor(ISqlGenerator sqlGenerator)
        {
            SqlGenerator = sqlGenerator;
        }

        public ISqlGenerator SqlGenerator { get; private set; }

        public TEntity Get<TEntity>(IDbConnection connection, dynamic id, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate predicate = GetIdPredicate(classMap, id);
            TEntity result = GetList<TEntity>(connection, classMap, predicate, null, transaction, commandTimeout, true).SingleOrDefault();
            return result;
        }

        public void Insert<TEntity>(IDbConnection connection, IEnumerable<TEntity> entities, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            string sql = GetInsertQueryUpdateColumnValue<TEntity>(classMap, entities);

            connection.Execute(sql, entities, transaction, commandTimeout, CommandType.Text);
        }

        public dynamic Insert<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            List<IPropertyMap> nonIdentityKeyProperties = classMap.Properties.Where(p => p.KeyType == KeyType.Guid || p.KeyType == KeyType.Assigned).ToList();
            var identityColumn = classMap.Properties.SingleOrDefault(p => p.KeyType == KeyType.Identity);
            IDictionary<string, object> keyValues = new ExpandoObject();
            string sql = GetInsertQueryUpdateColumnValue<TEntity>(classMap, nonIdentityKeyProperties, entity);

            if (identityColumn != null)
            {
                IEnumerable<long> result;
                if (SqlGenerator.SupportsMultipleStatements())
                {
                    sql += SqlGenerator.Configuration.Dialect.BatchSeperator + SqlGenerator.IdentitySql(classMap);
                    result = connection.Query<long>(sql, entity, transaction, false, commandTimeout, CommandType.Text);
                }
                else
                {
                    connection.Execute(sql, entity, transaction, commandTimeout, CommandType.Text);
                    sql = SqlGenerator.IdentitySql(classMap);
                    result = connection.Query<long>(sql, entity, transaction, false, commandTimeout, CommandType.Text);
                }

                long identityValue = result.First();
                // int identityInt = Convert.ToInt32(identityValue);
                keyValues.Add(identityColumn.Name, identityValue);
                identityColumn.PropertyInfo.SetValue(entity, identityValue, null);
            }
            else
            {
                connection.Execute(sql, entity, transaction, commandTimeout, CommandType.Text);
            }

            return GetInsertKeyValues<TEntity>(keyValues, nonIdentityKeyProperties, entity);
        }

        public bool Update<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            var predicate = GetPredicateForUpdate<TEntity>(classMap, entity);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Update(classMap, predicate, parameters);
            DynamicParameters dynamicParameters = new DynamicParameters();

            dynamicParameters = GetDynamicParametersForUpdate<TEntity>(classMap, parameters, entity);

            return connection.Execute(sql, dynamicParameters, transaction, commandTimeout, CommandType.Text) > 0;
        }

        public bool Delete<TEntity>(IDbConnection connection, TEntity entity, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate predicate = GetKeyPredicate<TEntity>(classMap, entity);
            return Delete<TEntity>(connection, classMap, predicate, transaction, commandTimeout);
        }

        public bool Delete<TEntity>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return Delete<TEntity>(connection, classMap, wherePredicate, transaction, commandTimeout);
        }

        public IEnumerable<TEntity> GetList<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return GetList<TEntity>(connection, classMap, wherePredicate, sort, transaction, commandTimeout, true);
        }

        public IEnumerable<TEntity> GetPage<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return GetPage<TEntity>(connection, classMap, wherePredicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered);
        }

        public IEnumerable<TEntity> GetSet<TEntity>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            return GetSet<TEntity>(connection, classMap, wherePredicate, sort, firstResult, maxResults, transaction, commandTimeout, buffered);
        }

        public int Count<TEntity>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            IClassMapper classMap = SqlGenerator.Configuration.GetMap<TEntity>();
            IPredicate wherePredicate = GetPredicate(classMap, predicate);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Count(classMap, wherePredicate, parameters);

            return (int)connection.Query(sql, GetDynamicParameters(parameters), transaction, false, commandTimeout, CommandType.Text).Single().Total;
        }

        public IMultipleResultReader GetMultiple(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            if (SqlGenerator.SupportsMultipleStatements())
            {
                return GetMultipleByBatch(connection, predicate, transaction, commandTimeout);
            }

            return GetMultipleBySequence(connection, predicate, transaction, commandTimeout);
        }

        protected IEnumerable<TEntity> GetList<TEntity>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Select(classMap, predicate, sort, parameters);

            return connection.Query<TEntity>(sql, GetDynamicParameters(parameters), transaction, buffered, commandTimeout, CommandType.Text);
        }

        protected IEnumerable<TEntity> GetPage<TEntity>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.SelectPaged(classMap, predicate, sort, page, resultsPerPage, parameters);

            return connection.Query<TEntity>(sql, GetDynamicParameters(parameters), transaction, buffered, commandTimeout, CommandType.Text);
        }

        protected IEnumerable<TEntity> GetSet<TEntity>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered) where TEntity : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.SelectSet(classMap, predicate, sort, firstResult, maxResults, parameters);

            return connection.Query<TEntity>(sql, GetDynamicParameters(parameters), transaction, buffered, commandTimeout, CommandType.Text);
        }

        protected bool Delete<TEntity>(IDbConnection connection, IClassMapper classMap, IPredicate predicate, IDbTransaction transaction, int? commandTimeout) where TEntity : class
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string sql = SqlGenerator.Delete(classMap, predicate, parameters);

            return connection.Execute(sql, GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text) > 0;
        }

        protected IPredicate GetPredicate(IClassMapper classMap, object predicate)
        {
            IPredicate wherePredicate = predicate as IPredicate;
            if (wherePredicate == null && predicate != null)
            {
                wherePredicate = GetEntityPredicate(classMap, predicate);
            }

            return wherePredicate;
        }

        protected IPredicate GetIdPredicate(IClassMapper classMap, object id)
        {
            bool isSimpleType = ReflectionHelper.IsSimpleType(id.GetType());
            var keys = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey && p.KeyType != KeyType.ConcurrencyCheck);
            IDictionary<string, object> paramValues = null;
            IList<IPredicate> predicates = new List<IPredicate>();
            if (!isSimpleType)
            {
                paramValues = ReflectionHelper.GetObjectValues(id);
            }

            foreach (var key in keys)
            {
                object value = id;
                if (!isSimpleType)
                {
                    value = paramValues[key.Name];
                }

                Type predicateType = typeof(FieldPredicate<>).MakeGenericType(classMap.EntityType);

                IFieldPredicate fieldPredicate = Activator.CreateInstance(predicateType) as IFieldPredicate;
                fieldPredicate.Not = false;
                fieldPredicate.Operator = Operator.Eq;
                fieldPredicate.PropertyName = key.Name;
                fieldPredicate.Value = value;
                predicates.Add(fieldPredicate);
            }

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected IPredicate GetConcurrencyPredicate<TEntity>(IClassMapper classMap, TEntity entity) where TEntity : class
        {
            var whereFields = classMap.Properties.Where(p => p.KeyType == KeyType.ConcurrencyCheck);
            if (!whereFields.Any())
            {
                return null;
            }

            IList<IPredicate> predicates = (from field in whereFields
                                            select new FieldPredicate<TEntity>
                                            {
                                                Not = false,
                                                Operator = Operator.Eq,
                                                PropertyName = field.Name,
                                                Value = field.PropertyInfo.GetValue(entity, null)
                                            }).Cast<IPredicate>().ToList();

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected IPredicate GetKeyPredicate<TEntity>(IClassMapper classMap, TEntity entity) where TEntity : class
        {
            var whereFields = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey && p.KeyType != KeyType.ConcurrencyCheck);
            if (!whereFields.Any())
            {
                throw new ArgumentException("At least one Key column must be defined.");
            }

            IList<IPredicate> predicates = (from field in whereFields
                                            select new FieldPredicate<TEntity>
                                            {
                                                Not = false,
                                                Operator = Operator.Eq,
                                                PropertyName = field.Name,
                                                Value = field.PropertyInfo.GetValue(entity, null)
                                            }).Cast<IPredicate>().ToList();

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected IPredicate GetEntityPredicate(IClassMapper classMap, object entity)
        {
            Type predicateType = typeof(FieldPredicate<>).MakeGenericType(classMap.EntityType);
            IList<IPredicate> predicates = new List<IPredicate>();
            foreach (var kvp in ReflectionHelper.GetObjectValues(entity))
            {
                IFieldPredicate fieldPredicate = Activator.CreateInstance(predicateType) as IFieldPredicate;
                fieldPredicate.Not = false;
                fieldPredicate.Operator = Operator.Eq;
                fieldPredicate.PropertyName = kvp.Key;
                fieldPredicate.Value = kvp.Value;
                predicates.Add(fieldPredicate);
            }

            return predicates.Count == 1
                       ? predicates[0]
                       : new PredicateGroup
                       {
                           Operator = GroupOperator.And,
                           Predicates = predicates
                       };
        }

        protected GridReaderResultReader GetMultipleByBatch(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            var sql = GetQueryForMulitpleByBatch(predicate, parameters);

            SqlMapper.GridReader grid = connection.QueryMultiple(sql.ToString(), GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text);
            return new GridReaderResultReader(grid);
        }

        protected SequenceReaderResultReader GetMultipleBySequence(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout)
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

                SqlMapper.GridReader queryResult = connection.QueryMultiple(sql, GetDynamicParameters(parameters), transaction, commandTimeout, CommandType.Text);
                items.Add(queryResult);
            }

            return new SequenceReaderResultReader(items);
        }

        protected StringBuilder GetQueryForMulitpleByBatch(GetMultiplePredicate predicate, IDictionary<string, object> parameters)
        {
            StringBuilder sql = new StringBuilder();
            foreach (var item in predicate.Items)
            {
                IClassMapper classMap = SqlGenerator.Configuration.GetMap(item.Type);
                IPredicate itemPredicate = item.Value as IPredicate;
                if (itemPredicate == null && item.Value != null)
                {
                    itemPredicate = GetPredicate(classMap, item.Value);
                }

                sql.AppendLine(SqlGenerator.Select(classMap, itemPredicate, item.Sort, parameters) + SqlGenerator.Configuration.Dialect.BatchSeperator);
            }

            return sql;
        }

        protected DynamicParameters GetDynamicParameters(IDictionary<string, object> parameters)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return dynamicParameters;
        }

        protected DynamicParameters GetDynamicParametersForUpdate<TEntity>(IClassMapper classMap, IDictionary<string, object> parameters, TEntity entity) where TEntity : class
        {
            DynamicParameters dynamicParameters = new DynamicParameters();

            // update the concurrency parameter
            var concurrencyColumns = classMap.Properties.Where(p => p.KeyType == KeyType.ConcurrencyCheck);
            foreach (var property in ReflectionHelper.GetObjectValues(entity).Where(property => concurrencyColumns.Any(c => c.Name == property.Key)))
            {
                var updatedValue = UpdateConcurrencyColumn(property.Value);
                entity.GetType().GetProperty(property.Key).SetValue(entity, updatedValue);
                dynamicParameters.Add(property.Key, updatedValue);
            }

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity || p.KeyType == KeyType.ConcurrencyCheck));
            foreach (var property in ReflectionHelper.GetObjectValues(entity).Where(property => columns.Any(c => c.Name == property.Key)))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return dynamicParameters;
        }

        protected string GetInsertQueryUpdateColumnValue<TEntity>(IClassMapper classMap, IEnumerable<TEntity> entities) where TEntity : class
        {
            var properties = classMap.Properties.Where(p => p.KeyType != KeyType.NotAKey && p.KeyType != KeyType.ConcurrencyCheck);

            foreach (var e in entities)
            {
                foreach (var column in properties)
                {
                    if (column.KeyType == KeyType.Guid)
                    {
                        Guid comb = SqlGenerator.Configuration.GetNextGuid();
                        column.PropertyInfo.SetValue(e, comb, null);
                    }
                }
            }

            return SqlGenerator.Insert(classMap);
        }

        protected string GetInsertQueryUpdateColumnValue<TEntity>(IClassMapper classMap, List<IPropertyMap> nonIdentityKeyProperties, TEntity entity) where TEntity : class
        {
            foreach (var column in nonIdentityKeyProperties)
            {
                if (column.KeyType == KeyType.Guid)
                {
                    Guid comb = SqlGenerator.Configuration.GetNextGuid();
                    column.PropertyInfo.SetValue(entity, comb, null);
                }
            }

            return SqlGenerator.Insert(classMap);
        }

        protected dynamic GetInsertKeyValues<TEntity>(IDictionary<string, object> keyValues, List<IPropertyMap> nonIdentityKeyProperties, TEntity entity) where TEntity : class
        {
            foreach (var column in nonIdentityKeyProperties)
            {
                keyValues.Add(column.Name, column.PropertyInfo.GetValue(entity, null));
            }

            if (keyValues.Count == 1)
            {
                return keyValues.First().Value;
            }

            return keyValues;
        }

        protected IPredicate GetPredicateForUpdate<TEntity>(IClassMapper classMap, TEntity entity) where TEntity : class
        {
            IPredicate keypredicate = GetKeyPredicate<TEntity>(classMap, entity);
            IPredicate concurrencyPredicate = GetConcurrencyPredicate<TEntity>(classMap, entity);
            var predicate = concurrencyPredicate == null ? keypredicate : new PredicateGroup
            {
                Operator = GroupOperator.And,
                Predicates = new[] { keypredicate, concurrencyPredicate }
            };
            return predicate;
        }

        protected object UpdateConcurrencyColumn(object concurrencyValue)
        {
            /*if (concurrencyValue.GetType() == typeof(DateTime))
            {
                return  DateTime.UtcNow;
            }*/
            if (concurrencyValue.GetType() == typeof(int))
            {
                return ((int)concurrencyValue) + 1;
            }
            throw new NotSupportedException("ConcurrencyProperty.Value is not supported: " + concurrencyValue.GetType());
        }
    }
}
