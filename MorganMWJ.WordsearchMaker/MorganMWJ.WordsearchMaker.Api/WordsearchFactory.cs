namespace MorganMWJ.WordsearchMaker.Api;
public class WordsearchFactory : IWordsearchFactory
{
    public Wordsearch Create(WordSearchRequest request)
    {
        var ws = new Wordsearch(
                request.WordList,
                (uint)request.GridSize,
                request.GenerationSeed);

        if (request.LetterCase.HasValue)
        {
            ws.Case = request.LetterCase.Value;
        }
        if (request.Language.HasValue)
        {
            ws.Language = request.Language.Value;
        }
        return ws;
    }
}
