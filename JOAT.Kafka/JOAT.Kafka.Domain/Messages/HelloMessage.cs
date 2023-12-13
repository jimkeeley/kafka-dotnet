namespace JOAT.Kafka.Domain.Messages;

public class HelloMessage
{
    public string Text { get; set; } = default!;
}
public class MyJsonCoreSerializer : ISerializer
{
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly JsonWriterOptions _writerOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonCoreSerializer"/> class.
    /// </summary>
    /// <param name="options">Json serializer options</param>
    public MyJsonCoreSerializer(JsonSerializerOptions options)
    {
        _serializerOptions = options;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonCoreSerializer"/> class.
    /// </summary>
    /// <param name="writerOptions">Json writer options</param>
    public MyJsonCoreSerializer(JsonWriterOptions writerOptions)
    {
        _writerOptions = writerOptions;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonCoreSerializer"/> class.
    /// </summary>
    /// <param name="serializerOptions">Json serializer options</param>
    /// <param name="writerOptions">Json writer options</param>
    public MyJsonCoreSerializer(JsonSerializerOptions serializerOptions, JsonWriterOptions writerOptions)
    {
        _serializerOptions = serializerOptions;
        _writerOptions = writerOptions;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonCoreSerializer"/> class.
    /// </summary>
    public MyJsonCoreSerializer()
        : this(new JsonSerializerOptions(), default)
    {
    }

    /// <inheritdoc/>
    public Task SerializeAsync(object message, Stream output, ISerializerContext context)
    {
        using var writer = new Utf8JsonWriter(output, _writerOptions);

        JsonSerializer.Serialize(writer, message, _serializerOptions);

        return Task.CompletedTask;
    }
}
public class MyJsonCoreDeserializer : IDeserializer
{
    private readonly JsonSerializerOptions _serializerOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonCoreDeserializer"/> class.
    /// </summary>
    /// <param name="options">Json serializer options</param>
    public MyJsonCoreDeserializer(JsonSerializerOptions options)
    {
        _serializerOptions = options;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonCoreDeserializer"/> class.
    /// </summary>
    public MyJsonCoreDeserializer()
        : this(new JsonSerializerOptions())
    {
    }

    /// <inheritdoc/>
    public async Task<object> DeserializeAsync(Stream input, Type type, ISerializerContext context)
    {
        return await JsonSerializer
            .DeserializeAsync(input, type, _serializerOptions)
            .ConfigureAwait(false);
    }
}