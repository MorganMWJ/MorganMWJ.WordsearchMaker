namespace MorganMWJ.WordsearchMaker;

[Flags]
public enum WordDirection
{
    Horizontal = 1, // 0001
    Vertical = 2, // 0010
    Diagonal = 4, // 0100
    NoReverse = 8  // 1000
}
