namespace MorganMWJ.WordsearchMaker.Api;

internal class WordSearchResponse
{
    public required int GridSize { get; set; }
    public required char[][] Grid { get; set; }
    public required List<string> Words { get; set; } = new();
    public int? Seed { get; set; }

    public string WordsearchString { get; set; } = string.Empty;
    public string SolutionString { get; set; } = string.Empty;
}