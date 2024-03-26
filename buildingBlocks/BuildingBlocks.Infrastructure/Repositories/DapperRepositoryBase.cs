using BuildingBlocks.Application.Contracts.Persistence;
using BuildingBlocks.Core.Domain.Entities;
using BuildingBlocks.Dapper.Contexts;
using BuildingBlocks.Dapper.Extensions;
using BuildingBlocks.Dapper.Extensions.Mappers;
using Dapper;
using Microsoft.Data.SqlClient;
using SqlKata;
using System.Data;

namespace BuildingBlocks.Infrastructure.Repositories
{
    public class DapperRepositoryBase<TEntity, TPrimaryKey> 
        : IDapperRepository<TEntity, TPrimaryKey> where TEntity : BaseAuditEntity<TPrimaryKey>
    {
        public DapperRepositoryBase(IDapperContext context)
        {
            Context = context;
            TableName = Formatting.Pluralize(typeof(TEntity).Name);
        }   

        public string TableName { get; }

        public IDapperContext Context { get; }

        public virtual async Task CreateAsync(TEntity entity)
        {
            entity.Version = 1;
            entity.CreatedOn = DateTime.UtcNow;
            entity.LastModifiedOn = DateTime.UtcNow;

            var generatedIdentity = await Context.Connection.InsertAsync(entity, Context.Transaction);

            // TODO: Check do we need to assign identity value manually
            entity.Id = (TPrimaryKey)generatedIdentity;
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            return await Context.Connection.DeleteAsync(entity, Context.Transaction);
        }

        public virtual async Task<List<TEntity>> GetAsync()
        {
            return (await Context.Connection.GetListAsync<TEntity>()).AsList();
        }

        public virtual async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return await Context.Connection.GetAsync<TEntity>(id, Context.Transaction);
        }

        public async Task<List<TEntity>> GetByQueryAsync(Query query)
        {
            return (await Context.Connection.GetListAsync<TEntity>(query)).ToList();
        }

        public async Task<TEntity> GetSingleOrDefaultAsync(Query query, object param)
        {
            var selectStatement = Context.Compiler.Compile(query).ToString();
            return await Context.Connection.QuerySingleOrDefaultAsync<TEntity>(selectStatement, param, Context.Transaction);
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            entity.Version = entity.Version + 1;
            entity.LastModifiedOn = DateTime.UtcNow;

            return await Context.Connection.UpdateAsync(entity, Context.Transaction);
        }

        public IDbConnection CreateConnection
            => new SqlConnection(Context.Connection.ConnectionString);
    }
}