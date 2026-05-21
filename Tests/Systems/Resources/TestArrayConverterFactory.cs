using System.Text.Json;
using Termule.Engine.Systems.Resources;

namespace Termule.Tests.Systems.Resources;

public class TestArrayConverterFactory
{
    public static readonly TheoryData<string[,], string> WriteData = new()
    {
        { new string[0, 0], "[]" },
        {
            new[,] { { "Test" } },
            """
            [
                [
                    "Test"
                ]
            ]
            """
        },
        {
            new[,] { { "A", "B" } },
            """
            [
                [
                    "A",
                    "B"
                ]
            ]
            """
        },
        {
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
        },
        {
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
        }
    };

    public static readonly TheoryData<string, string[,]> ReadData = new()
    {
        { "[]", new string[0, 0] },
        {
            """
            [
                [
                    "Test"
                ]
            ]
            """,
            new[,] { { "Test" } }
        },
        {
            """
            [
                [
                    "A",
                    "B"
                ]
            ]
            """,
            new[,] { { "A", "B" } }
        },
        {
            """
            [
                [
                    "A"
                ],
                [
                    "B"
                ]
            ]
            """,
            new[,] { { "A" }, { "B" } }
        },
        {
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
            """,
            new[,] { { "A", "B", "C" }, { "D", "E", "F" } }
        }
    };

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new Array2DConverterFactory() },
        WriteIndented = true,
        IndentSize = 4,
        NewLine = "\n"
    };

    [Theory]
    [MemberData(nameof(WriteData))]
    public void Write_CorrectlyConvertsArrayToJson(string[,] array, string expected)
    {
        Assert.Equal(expected, JsonSerializer.Serialize(array, SerializerOptions));
    }

    [Theory]
    [MemberData(nameof(ReadData))]
    public void Read_CorrectlyConvertsJsonToArray(string json, string[,] expected)
    {
        Assert.Equal(expected, JsonSerializer.Deserialize<string[,]>(json, SerializerOptions));
    }
}
