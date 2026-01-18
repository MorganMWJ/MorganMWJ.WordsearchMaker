namespace MorganMWJ.WordsearchMaker.Api;
public static class WordsearchExtensions
{
    public static WordSearchResponse ToResponse(this Wordsearch wordsearch)
    {
        var wsGridJagged = wordsearch.ToJaggedGrid();

        return new WordSearchResponse
        {
            GridSize = (int)wordsearch.Size,
            Grid = wsGridJagged,
            Words = wordsearch.Words,
            Seed = wordsearch.Seed,
            WordsearchString = wordsearch.ToStringWordsearch(),
            SolutionString = wordsearch.ToStringSolution()
        };
    }

    /// <summary>
    /// Move this into nuget pkg later
    /// </summary>
    public static char[][] ToJaggedGrid(this Wordsearch wordsearch)
    {
        var source = wordsearch.GetGrid();
        int rows = source.GetLength(0);
        int cols = source.GetLength(1);

        var result = new char[rows][];

        for (int i = 0; i < rows; i++)
        {
            result[i] = new char[cols];
            for (int j = 0; j < cols; j++)
            {
                result[i][j] = source[i, j];
            }
        }

        return result;
    }
}
