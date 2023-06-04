using System.Text.Json;
using Confluent.Kafka;
using TimMovie.Core.Events;

namespace TimMovie.Infrastructure.Serializers;

public class RegisterFilmTrafficEventSerializer : IDeserializer<RegisterFilmTrafficEvent>, ISerializer<RegisterFilmTrafficEvent>
{
    public byte[] Serialize(RegisterFilmTrafficEvent data, SerializationContext context)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (data == null)
        {
            return null!;
        }

        var bytes = JsonSerializer.SerializeToUtf8Bytes(data);

        return bytes;
    }

    public RegisterFilmTrafficEvent Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return null!;
        }

        var json = System.Text.Encoding.UTF8.GetString(data.ToArray());
        var message = JsonSerializer.Deserialize<RegisterFilmTrafficEvent>(json);

        return message!;
    }
}