using System.Text;
using System.Text.RegularExpressions;

namespace MorganMWJ.WordsearchMaker;

/// <summary>
/// To build a wordsearch puzzle
/// Call the constructor then add words OR
/// Call the constructor with a list of words
/// THEN call Regenerate to build the grid
/// </summary>
public class Wordsearch
{
    /// <summary>
    /// Size of the wordsearch square grid (Size x Size).
    /// </summary>
    public uint Size { get; }

    /// <summary>
    /// Random seed used to generate the wordsearch grid.
    /// If unset Regenerate will use a random seed each time.
    /// </summary>
    public int? Seed { get; }

    /// <summary>
    /// The list of words in the wordsearch.
    /// </summary>
    public List<string> Words => _wordsPositions.Keys.ToList();

    private LetterCase _case = LetterCase.Lower;

    /// <summary>
    /// The letter case of the wordsearch.
    /// </summary>
    public LetterCase Case
    {
        get => _case;
        set
        {
            if (_case != value)
            {
                _case = value;
                UpdateWordCase();
            }
        }
    }

    private Language _language = Language.English;

    /// <summary>
    /// The language of the wordsearch.
    /// </summary>
    public Language Language
    {
        get => _language;
        set
        {
            if (_language != value)
            {
                _language = value;
                _wordsPositions.Clear();
            }
        }
    }

    /// <summary>
    /// The allowed directions for words in the wordsearch.
    /// This is a Flagged enum, so multiple directions can be combined.
    /// </summary>
    public WordDirection AllowedDirection { get; set; } = WordDirection.Horizontal | WordDirection.Vertical | WordDirection.Diagonal;

    internal char[,] _grid;

    internal Dictionary<string, List<Vector>> _wordsPositions = new Dictionary<string, List<Vector>>();

    private int _regenerateCount = 0;
    private const char Empty = ' ';
    private const char Mask = '_';

    public Wordsearch(uint size = 30, int? seed = null)
    {
        if (size < 8 || size > 70)
            throw new ArgumentOutOfRangeException("Size must be between 8 and 70.");

        Seed = seed;
        RandomProvider.Current = Seed.HasValue ? new Random(Seed.Value) : new Random();
        Size = size;
        _grid = new char[size,size];
        _wordsPositions = new Dictionary<string, List<Vector>>();
    }

    public Wordsearch(List<string> words, uint size = 30, int? seed = null) : this(size, seed)
    {
        AddWords(words);
    }

    public void AddWords(List<string> words)
    {
        foreach (string word in words)
        {
            AddWord(word);
        }
    }

    public void AddWord(string word)
    {
        if (word.Length > Size)
            throw new ArgumentException($"The word '{word}' is too long for the grid size {Size}x{Size}.");

        if (Language.IsInvalidWord(word))
            throw new ArgumentException($"Input word '{word}' must contain only contain valid {Language} alphabet characters.");

        // case
        if (Case == LetterCase.Upper)
            word = word.ToUpper(Language.GetCultureInfo());
        else
            word = word.ToLower(Language.GetCultureInfo());

        _wordsPositions.Add(word, new List<Vector>());
    }

    private void PlaceWords()
    {
        foreach (var word in _wordsPositions.Keys)
        {
            PlaceWord(word);
        }
    }

    private void PlaceWord(string word)
    {
        int maxAttempts = Math.Max(1000, (int)(Size * Size * 0.5));

        // select position & direction at random
        var pos = Vector.RandomPosition((int)Size);
        var dir = Direction.RandomDirection(AllowedDirection);
        var placementAttempts = 1;

        // check if word fits in that direction
        while (!Fits(word, pos, dir))
        {
            // if not - try another position & direction
            pos = Vector.RandomPosition((int)Size);
            dir = Direction.RandomDirection(AllowedDirection);
            placementAttempts++;

            // after too many attempts, give up - something wrong            
            if (placementAttempts > maxAttempts)
                throw new InvalidOperationException($"Could not place the word '{word}' in the grid after {placementAttempts} attempts. Try increasing the grid size or changing the word list.");
        }

        // if it does, add it to the grid
        InsertWord(word, pos, dir);
    }

    /// <summary>
    /// Checks if word fits in the grid at the given position and direction
    /// </summary>
    /// <param name="word"></param>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    private bool Fits(string word, Vector pos, Vector dir)
    {
        try
        {
            var currentPos = pos;
            foreach (char c in word)
            {
                if (c == _grid[currentPos.X, currentPos.Y] || Empty == _grid[currentPos.X, currentPos.Y])
                {
                    currentPos = new Vector(currentPos.X + dir.X, currentPos.Y + dir.Y);
                }
                else
                {
                    return false;
                }

            }

            return true;
        }
        catch(Exception)
        {
            // this will happen often when the position/direction selected
            // results in the word going out of bounds of the grid
            return false;
        }
    }

    /// <summary>
    /// Inserts word into the grid at the given position and direction.
    /// Mutating grid and wordsPositions.
    /// </summary>
    /// <param name="word"></param>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    private void InsertWord(string word, Vector pos, Vector dir)
    {
        _wordsPositions[word].Clear();
        var currentPos = pos;

        foreach (char c in word)
        {
            if (c == _grid[currentPos.X, currentPos.Y])
            {
                _wordsPositions[word].Add(new Vector(currentPos.X, currentPos.Y));
            }
            else if(Empty == _grid[currentPos.X, currentPos.Y])
            {
                _grid[currentPos.X, currentPos.Y] = c;
                _wordsPositions[word].Add(new Vector(currentPos.X, currentPos.Y));
            }
            else
            {
                throw new Exception("Should be impossible as fit called first: Word does not fit in the grid.");
            }

            currentPos = new Vector(currentPos.X + dir.X, currentPos.Y + dir.Y);
        }
    }

    /// <summary>
    /// Init the grid with empty space characters.
    /// </summary>
    private void InitializeGrid()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                _grid[i, j] = Empty;
            }
        }
    }

    private void FillGrid()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (_grid[i, j] == Empty)
                {
                    _grid[i, j] = Language.GetAlphabet(Case).RandChar();
                }
            }
        }
    }

    private void UpdateWordCase()
    {
        var newDict = new Dictionary<string, List<Vector>>();
        foreach (var kvp in _wordsPositions)
        {
            string newKey = _case == LetterCase.Upper
                ? kvp.Key.ToUpper(Language.GetCultureInfo())
                : kvp.Key.ToLower(Language.GetCultureInfo());
            newDict[newKey] = kvp.Value;
        }
        _wordsPositions = newDict;
    }

    /// <summary>
    /// Re-builds the wordsearch grid with the current words.
    /// </summary>
    public void Regenerate()
    {
        // Ensure we are using the correct random seed if set
        RandomProvider.Current = Seed.HasValue ? new Random(Seed.Value) : new Random();

        InitializeGrid();

        PlaceWords();

        FillGrid();

        _regenerateCount++;
    }

    /// <summary>
    /// ToString wordsearch grid
    /// </summary>
    public string ToStringWordsearch()
    {
        if (_regenerateCount == 0)
            throw new InvalidOperationException("The wordsearch grid has not been generated yet. Please call Regenerate() before calling this method.");

        var sb = new StringBuilder();

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                sb.Append(_grid[i, j]);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Each line of the wordsearch as an element in an array of strings
    /// </summary>
    public string[] GetWordsearchLines()
    {
        var lines = ToStringWordsearch().Split(
            Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        return lines;
    }

    /// <summary>
    /// ToString only words in their positions
    /// </summary>
    public string ToStringSolution()
    {
        if(_regenerateCount == 0)
            throw new InvalidOperationException("The wordsearch grid has not been generated yet. Please call Regenerate() before calling this method.");

        var sb = new StringBuilder();
        var solutionPositions = _wordsPositions.Values.SelectMany(x => x);

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (solutionPositions.Contains(new Vector(i, j)))
                {
                    sb.Append(_grid[i, j]);
                }
                else
                {
                    sb.Append(Mask);
                }
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Each line of the solution as an element in an array of strings
    /// </summary>
    public string[] GetSolutionLines()
    {
        var lines = ToStringSolution().Split(
            Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        return lines;
    }

    /// <summary>
    /// Access to individual cell in the grid
    /// </summary>
    public char GetCell(int row, int column)
    {
        return _grid[row, column];
    }

    /// <summary>
    /// Return copy of the grid as a 2D char array
    /// </summary>
    /// <returns></returns>
    public char[,] GetGrid()
    {
        return (char[,])_grid.Clone();
    }
}
