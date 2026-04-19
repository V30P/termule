using System.Text.Json;

namespace Termule.Engine.Systems.Resources;

/// <summary>
///     Provides methods to serialize and deserialize objects.
/// </summary>
public static class Serializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new Array2DConverterFactory() },
        WriteIndented = true
    };

    /// <summary>
    ///     Serializes the provided value.
    /// </summary>
    /// <param name="value">The value to serialize.</param>
    /// <returns>The serialized value.</returns>
    public static string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, SerializerOptions);
    }

    /// <summary>
    ///     Deserializes the provided value as <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the value as.</typeparam>
    /// <param name="serializedValue">The value to deserialize.</param>
    /// <returns>The deserialized value.</returns>
    public static T Deserialize<T>(string serializedValue)
    {
        return JsonSerializer.Deserialize<T>(serializedValue, SerializerOptions);
    }
}