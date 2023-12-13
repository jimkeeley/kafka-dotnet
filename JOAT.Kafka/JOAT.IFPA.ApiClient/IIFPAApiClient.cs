namespace JOAT.IFPA.ApiClient;

public interface IIFPAApiClient
{
    Task<RankingsResponse?> GetRankingsAsync(RankingsRequest rankingsRequest);
}