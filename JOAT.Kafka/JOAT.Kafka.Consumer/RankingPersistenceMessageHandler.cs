namespace JOAT.Kafka.Consumer;

public class RankingPersistenceMessageHandler : IMessageHandler<RankingMessage>
{
    public Task Handle(IMessageContext context, RankingMessage message)
    {
        //TODO:Add handler to persist ratings data to db (should be upsert by PlayerId)
        Console.WriteLine(
            "Partition: {0} | Offset: {1} | Message: {2}",
            context.ConsumerContext.Partition,
            context.ConsumerContext.Offset,
            message.Ranking);

        return Task.CompletedTask;
    }
}