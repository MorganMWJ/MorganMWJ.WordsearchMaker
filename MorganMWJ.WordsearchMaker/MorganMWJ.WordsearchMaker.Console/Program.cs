using System.Text;

namespace MorganMWJ.WordsearchMaker.Console;

internal class Program
{
    static void Main(string[] args)
    {
        var w = new Wordsearch(40);

        w.AddWord("EXAMPLE");
        w.AddWord("tester");
        w.AddWord("coolguy");
        w.Regenerate();

        //w.PrintWordsearch();
        //w.PrintSolution();

        System.Console.WriteLine(w.ToStringWordsearch());
        System.Console.WriteLine(w.ToStringSolution());

        w.Case = LetterCase.Upper;
        w.Regenerate();
        System.Console.WriteLine(w.ToStringWordsearch());
        System.Console.WriteLine(w.ToStringSolution());

        var w2 = new Wordsearch(15, seed: 123);
        w2.AddWords(new List<string>
        {
            "apple",
            "banana",
            "cherry",
            "date",
            "fig",
            "grape",
            "kiwi"
        });
        w2.Case = LetterCase.Lower;
        w2.AllowedDirection = WordDirection.Horizontal;
        w2.Regenerate();
        System.Console.WriteLine(w2.ToStringWordsearch());
        System.Console.WriteLine(w2.ToStringSolution());

        var w2_duplicate = new Wordsearch(15, seed: 123);
        w2_duplicate.AddWords(new List<string>
        {
            "apple",
            "banana",
            "cherry",
            "date",
            "fig",
            "grape",
            "kiwi"
        });
        w2_duplicate.Case = LetterCase.Lower;
        w2_duplicate.AllowedDirection = WordDirection.Horizontal;
        w2_duplicate.Regenerate();
        System.Console.WriteLine(w2_duplicate.ToStringWordsearch());
        System.Console.WriteLine(w2_duplicate.ToStringSolution());


        System.Console.OutputEncoding = Encoding.UTF8;
        System.Console.InputEncoding = Encoding.UTF8;
        var polish = new Wordsearch(15, seed: 123);

        polish.Case = LetterCase.Lower;
        polish.Language = Language.Polish;
        polish.AddWords(new List<string>
        {
            "książka",
            "przyjaciel",
            "samochód",
            "dziewczyna",
            "rowerzysta",
            "jabłko",
            "pogoda"
        });
        
        polish.Regenerate();
        System.Console.WriteLine(polish.ToStringWordsearch());
        System.Console.WriteLine(polish.ToStringSolution());
    }
}
