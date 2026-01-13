namespace MorganMWJ.WordsearchMaker;
internal struct Vector
{
    /// <summary>
    /// Position in top level array
    /// </summary>
    internal int X { get; }

    /// <summary>
    /// Position in second level array
    /// </summary>
    internal int Y { get; }

    internal Vector(int x, int y)
    {
        X = x;
        Y = y;
    }

    internal static Vector RandomPosition(int size)
    {
        return new Vector(size.Rand(), size.Rand());
    }
}