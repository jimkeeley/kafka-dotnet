using JOAT.IFPA.ApiClient;

namespace JOAT.Data;

public interface IRankingsRepository
{
    /// <summary>
    /// User Stored Proc to Upsert Ranking Record
    /// into Sql Server
    /// </summary>
    /// <param name="ranking"></param>
    /// <returns></returns>
    Task<Ranking> UpsertAsync(Ranking ranking);

    /// <summary>
    /// User Stored Proc to Upsert Ranking Record
    /// into Sql Server
    /// </summary>
    /// <param name="rankings"></param>
    /// <returns></returns>
    Task<List<Ranking>> BulkUpsertAsync(List<Ranking> rankings);
}