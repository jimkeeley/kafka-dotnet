namespace JOAT.IFPA.ApiClient;

public class RankingsResponse(List<Ranking> rankings)
{
    [JsonPropertyName("total_count")]
    public long? TotalCount { get; set; }

    [JsonPropertyName("sub_category")]
    public string? SubCategory { get; set; }

    [JsonPropertyName("rankings")]
    public List<Ranking>? Rankings { get; set; } = rankings;
}