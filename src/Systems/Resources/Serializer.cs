using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termule.Resources;

public static class Serializer
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters = { new Array2DConverterFactory() },
        WriteIndented = true
    };

    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, _serializerOptions);
    }

    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _serializerOptions);
    }
}

internal class Array2DConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsArray && typeToConvert.GetArrayRank() == 2;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type converterType = typeof(Array2DConverter<>).MakeGenericType([typeToConvert.GetElementType()]);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }

    private class Array2DConverter<T> : JsonConverter<T[,]>
    {
        private static JsonConverter<T> GetConverter(JsonSerializerOptions options)
        {
            return (JsonConverter<T>)options.GetConverter(typeof(T));
        }

        public override T[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter<T> converter = GetConverter(options);

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();

            // Read the array as a list of lists
            List<List<T>> array = [];
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                reader.Read();
                List<T> subarray = [];

                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    subarray.Add(converter.Read(ref reader, typeToConvert.GetElementType(), options));
                    reader.Read();
                }

                reader.Read();
                array.Add(subarray);
            }

            // Convert to an actual array (or null)
            T[,] value = array.Count != 0 ? new T[array.Count, array[0].Count] : null;
            for (int x = 0; x < array.Count; x++)
            {
                for (int y = 0; y < array[0].Count; y++)
                {
                    value[x, y] = array[x][y];
                }
            }

            return value;
        }

        public override void Write(Utf8JsonWriter writer, T[,] value, JsonSerializerOptions options)
        {
            JsonConverter<T> converter = GetConverter(options);

            writer.WriteStartArray();
            for (int x = 0; x < value.GetLength(0); x++)
            {
                writer.WriteStartArray();
                for (int y = 0; y < value.GetLength(1); y++)
                {
                    converter.Write(writer, value[x, y], options);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
    }
}