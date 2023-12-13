namespace JOAT.Kafka.RatingsConsumer;

public class RankingPersistenceMessageHandler : IMessageHandler<Ranking>
{
    private readonly IRankingsRepository _rankingsRepository;

    public RankingPersistenceMessageHandler(IRankingsRepository rankingsRepository)
    {
        _rankingsRepository = rankingsRepository;
    }
    public async Task Handle(IMessageContext context, Ranking message)
    {
        var result = await _rankingsRepository.UpsertAsync(message);
    }
}