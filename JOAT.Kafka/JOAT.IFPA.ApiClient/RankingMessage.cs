using System.Runtime.Serialization;

namespace JOAT.IFPA.ApiClient;

[DataContract]
public class RankingMessage
{
    [DataMember(Order = 1)]
    public Ranking Ranking { get; set; }
}