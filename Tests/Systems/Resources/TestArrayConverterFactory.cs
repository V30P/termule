using System.Text.Json;
using Termule.Engine.Systems.Resources;

namespace Termule.Tests.Systems.Resources;

public class TestArrayConverterFactory
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new Array2DConverterFactory() }, WriteIndented = true
    };

    public static readonly IEnumerable<object[]> WriteData =
    [
        [new string[0, 0], "[]"],
        [
            new[,] { { "Test" } },
            """
            [
              [
                "Test"
              ]
            ]
            """
        ],
        [
            new[,] { { "A", "B" } },
            """
            [
              [
                "A",
                "B"
              ]
            ]
            """
        ],
        [
            new[,] { { "A" }, { "B" } },
            """
            [
              [
                "A"
              ],
              [
                "B"
              ]
            ]
            """
        ],
        [
            new[,] { { "A", "B", "C" }, { "D", "E", "F" } },
            """
            [
              [
                "A",
                "B",
                "C"
              ],
              [
                "D",
                "E",
                "F"
              ]
            ]
            """
        ]
    ];

    public static IEnumerable<object[]> ReadData =>
        WriteData.Select<object[], object[]>(o => [o[1], o[0]]);

    [Theory]
    [MemberData(nameof(ReadData))]
    public void Read_ShouldCorrectlyConvertJsonToArray(string json, string[,] expected)
    {
        Assert.Equal(expected, JsonSerializer.Deserialize<string[,]>(json, SerializerOptions));
    }

    [Theory]
    [MemberData(nameof(WriteData))]
    public void Write_ShouldCorrectlyConvertArrayToJson(string[,] array, string expected)
    {
        Assert.Equal(expected, JsonSerializer.Serialize(array, SerializerOptions));
    }
}