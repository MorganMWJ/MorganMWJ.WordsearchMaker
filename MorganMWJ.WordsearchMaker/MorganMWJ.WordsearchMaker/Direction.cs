namespace MorganMWJ.WordsearchMaker;

/// <summary>
/// Direction when accessing a 2d array
/// X is index of top level array
/// Y is index of second level array
/// </summary>
internal static class Direction
{
    internal static Vector Up = new Vector(-1, 0);
    internal static Vector UpRight = new Vector(-1, 1);
    internal static Vector Right = new Vector(0, 1);
    internal static Vector DownRight = new Vector(1, 1);
    internal static Vector Down = new Vector(1, 0);
    internal static Vector DownLeft = new Vector(1, -1);
    internal static Vector Left = new Vector(0, -1);
    internal static Vector UpLeft = new Vector(-1, -1);
    
    internal static Vector[] All = { Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft };

    /// <summary>
    /// Returns a random direction
    /// </summary>
    internal static Vector RandomDirection()
        => All.Rand();

    /// <summary>
    /// Returns a random direction restricted to allowed directions
    /// </summary>
    internal static Vector RandomDirection(WordDirection allowed)
    {
        var allowedVectors = new List<Vector>();

        if(allowed.HasFlag(WordDirection.Horizontal))
        {
            allowedVectors.Add(Right);
            allowedVectors.Add(Left);
        }

        if(allowed.HasFlag(WordDirection.Vertical))
        {
            allowedVectors.Add(Up);
            allowedVectors.Add(Down);
        }

        if(allowed.HasFlag(WordDirection.Diagonal))
        {
            allowedVectors.Add(UpRight);
            allowedVectors.Add(DownRight);
            allowedVectors.Add(DownLeft);
            allowedVectors.Add(UpLeft);
        }

        if(allowed.HasFlag(WordDirection.NoReverse))
        {
            allowedVectors.Remove(Left);
            allowedVectors.Remove(Up);
            allowedVectors.Remove(DownLeft);
            allowedVectors.Remove(UpLeft);
        }

        if(!allowedVectors.Any())
            throw new ArgumentException("No directions selected.", nameof(allowed));

        return allowedVectors.Rand();
    }
}
