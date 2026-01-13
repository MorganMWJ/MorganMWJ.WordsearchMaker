namespace MorganMWJ.WordsearchMaker;
internal static class RandomExtensions
{
    internal static int Rand(this int max)
        => RandomProvider.Current.Next(max);

    internal static T Rand<T>(this T[] array)
        => array[RandomProvider.Current.Next(array.Length)];

    internal static T Rand<T>(this IEnumerable<T> collection) =>
        !collection.Any() ? 
        throw new ArgumentException() :
        collection.ElementAt(RandomProvider.Current.Next(collection.Count()));

    internal static char RandChar(this string input)
        => input.ToCharArray().Rand();
    
}
