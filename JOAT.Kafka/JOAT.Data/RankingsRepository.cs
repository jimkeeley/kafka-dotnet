namespace JOAT.Data;

public class RankingsRepository: RepositoryBase, IRankingsRepository
{
    public RankingsRepository(IDbConnectionFactory connectionFactory) 
        : base(connectionFactory)
    {
    }

    /// <summary>
    /// User Stored Proc to Upsert Ranking Record
    /// into Sql Server
    /// </summary>
    /// <param name="ranking"></param>
    /// <returns></returns>
    public async Task<Ranking> UpsertAsync(Ranking ranking)
    {
        string sql = @"RankingUpsert";

        await this.DbConnection
            .ExecuteScalarAsync<string>(sql, new
            {
                data = new List<Ranking>(){ranking}.AsTableValuedParameter("dbo.RankingType")
            },commandType:CommandType.StoredProcedure)
            .ConfigureAwait(false);

        return ranking;
    }

    /// <summary>
    /// User Stored Proc to Upsert Ranking Record
    /// into Sql Server
    /// </summary>
    /// <param name="rankings"></param>
    /// <returns></returns>
    public async Task<List<Ranking>> BulkUpsertAsync(List<Ranking> rankings)
    {
        var sql = @"RankingUpsert";

        await this.DbConnection
            .ExecuteScalarAsync<string>(sql, new
            {
                data = rankings.AsTableValuedParameter("dbo.RankingType")
            }, commandType: CommandType.StoredProcedure)
            .ConfigureAwait(false);

        return rankings;
    }
}