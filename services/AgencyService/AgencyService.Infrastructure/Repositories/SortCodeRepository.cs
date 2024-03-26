using AgencyService.Application.Contracts.Persistence;
using AgencyService.Domain.Entities;
using BuildingBlocks.Dapper.Contexts;
using BuildingBlocks.Dapper.Extensions;
using BuildingBlocks.Infrastructure.Repositories;
using Dapper;
using SqlKata;
using System.Data;

namespace AgencyService.Infrastructure.Repositories
{
    public class SortCodeRepository : DapperRepositoryBase<SortCode, int>, ISortCodeRepository
    {
        public SortCodeRepository(IDapperContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<SortCode>> GetSortcodesForAgency(Guid agencyId)
        {
            var query = new Query(TableName).Where(nameof(SortCode.AgencyId), agencyId);
            return await base.GetByQueryAsync(query);
        }

        public async Task<List<SortCode>> GetSortcodesForAgencyUsingSp(Guid agencyId)
        {
            var procedureName = "spGetInstitutionSortCode";
            var parameters = new DynamicParameters();
            parameters.Add("Id", agencyId, DbType.Guid, ParameterDirection.Input);

            using (var connection = Context.Connection.CreateConnection())
            {
                var sortCodesForAgency = await Context.Connection.QueryAsync<SortCode>(
                    procedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return sortCodesForAgency != null ? sortCodesForAgency.ToList() : new List<SortCode>();
            }
        }

        public async Task<bool> VerifySortCodeForAgency(Guid agencyId, string sortCode)
        {
            var query = new Query(TableName)
                .Where(nameof(SortCode.AgencyId), agencyId)
                .Where(nameof(SortCode.Value), sortCode);

            var existingSortCodes = await base.GetByQueryAsync(query);

            return existingSortCodes.Any();
        }

        public async Task<List<AgencyIdSortCodeTuple>> GetSortCodeForAllAgencies()
        {
            //var query = new Query(TableName)
            //    .Select(nameof(SortCode.AgencyId))
            //    .Select(nameof(SortCode.Value));

            //var abs = @"SELECT SortCodes.Value, Agencies.UId
            //          FROM SortCodes
            //          INNER JOIN Agencies ON SortCodes.AgencyId = Agencies.Id";

            var testQuery = new Query(TableName)
                .Join("agencies",
                    _ => _.On(nameof(SortCode.AgencyId), nameof(Agency.Id)))
                .Where(nameof(Agency.Id), nameof(Agency.UId));

            //var sortCodes = await base.FindAsync(query);
            var sortCodes = await Context.Connection.GetListAsync<AgencyIdSortCodeTuple>(testQuery);

            return sortCodes.ToList() ?? new List<AgencyIdSortCodeTuple>();

            //var distinctAgencyIds = sortCodes
            //    .Select(_ => _.AgencyId)
            //    .Distinct()
            //    .ToList();

            //var agencyIdSortCodeTuples = new List<AgencyIdSortCodeTuple>();

            //distinctAgencyIds.ForEach(id => 
            //    agencyIdSortCodeTuples.Add(new AgencyIdSortCodeTuple
            //    {
            //        AgencyId = id,
            //        SortCodes = sortCodes
            //            .Where(_ => _.AgencyId == id)
            //            .Select(_ => _.Value)
            //            .ToList()
            //    }));

            //return agencyIdSortCodeTuples;
        }
    }
}
