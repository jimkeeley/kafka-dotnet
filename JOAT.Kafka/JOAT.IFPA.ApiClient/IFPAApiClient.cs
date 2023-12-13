namespace JOAT.IFPA.ApiClient;

public class IFPAApiClient(HttpClient httpClient, IFPAApiOptions ifpaApiOptions) : IIFPAApiClient
{
    public async Task<RankingsResponse?> GetRankingsAsync(RankingsRequest rankingsRequest)
    {
        var url = $"{ifpaApiOptions.ApiVersion}/rankings?api_key={ifpaApiOptions.ApiKey}&start_pos={rankingsRequest.StartPosition}&count={rankingsRequest.Count}&order={rankingsRequest.Order}";
        var response = await httpClient.GetFromJsonAsync<RankingsResponse>(url);
        return response;
    }
}