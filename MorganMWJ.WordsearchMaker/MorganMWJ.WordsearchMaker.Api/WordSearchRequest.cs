namespace MorganMWJ.WordsearchMaker.Api;

public class WordSearchRequest
{
    public int GridSize { get; set; }
    public List<string> WordList { get; set; } = [];
    public int? GenerationSeed { get; set; }

    public LetterCase? LetterCase { get; set; }

    public Language? Language { get; set; }
}
