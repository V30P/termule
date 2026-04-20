using System.Text.Json;

namespace Termule.Engine.Systems.Resources;

internal static class Serializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new Array2DConverterFactory() }, WriteIndented = true
    };

    internal static string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, SerializerOptions);
    }

    internal static T Deserialize<T>(string serializedValue)
    {
        return JsonSerializer.Deserialize<T>(serializedValue, SerializerOptions);
    }
}