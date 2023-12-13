using System.Runtime.Serialization;

namespace JOAT.IFPA.ApiClient;

[DataContract]
public class Ranking
{
    [JsonPropertyName("player_id")]
    [DataMember(Order = 1)]
    public string PlayerId { get; set; }

    [JsonPropertyName("first_name")]
    [DataMember(Order = 2)]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    [DataMember(Order = 3)]
    public string LastName { get; set; }

    [JsonPropertyName("age")]
    [DataMember(Order = 4)]
    public int Age { get; set; }

    [JsonPropertyName("country_name")]
    [DataMember(Order = 5)]
    public string CountryName { get; set; }

    [JsonPropertyName("country_code")]
    [DataMember(Order = 6)]
    public string CountryCode { get; set; }

    [JsonPropertyName("state")]
    [DataMember(Order = 7)]
    public string State { get; set; }

    [JsonPropertyName("city")]
    [DataMember(Order = 8)]
    public string City { get; set; }

    [JsonPropertyName("wppr_points")]
    [DataMember(Order = 9)]
    public string WpprPoints { get; set; }
 
    [JsonPropertyName("current_wppr_rank")]
    [DataMember(Order = 10)]
    public string CurrentWpprRank { get; set; }

    [JsonPropertyName("last_month_rank")]
    [DataMember(Order = 11)]
    public string LastMonthRank { get; set; }

    [JsonPropertyName("rating_value")]
    [DataMember(Order = 12)]
    public string RatingValue { get; set; }

    [JsonPropertyName("efficiency_percent")]
    [DataMember(Order = 13)]
    public string EfficiencyPercent { get; set; }

    [JsonPropertyName("event_count")]
    [DataMember(Order = 14)]
    public string EventCount { get; set; }

    [JsonPropertyName("best_finish")]
    [DataMember(Order = 15)]
    public string BestFinish { get; set; }

    [JsonPropertyName("best_finish_position")]
    [DataMember(Order = 16)]
    public string BestFinishPosition { get; set; }

    [JsonPropertyName("best_tournament_id")]
    [DataMember(Order = 17)]
    public string BestTournamentId { get; set; }
}