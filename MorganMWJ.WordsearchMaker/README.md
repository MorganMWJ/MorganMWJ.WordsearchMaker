# MorganMWJ.WordsearchMaker

Builds a wordsearch puzzle from a list of words.

- Call the constructor with a list of words and the size of the puzzle.
- OR Call the constructor with only a size and add words with the AddWord(s) method. 
- THEN call Regenerate() to build the puzzle.
- Subsequent calls to Regenerate() will rebuild the current puzzle using the same words.
- If seed is set same puzzle will be generated each time. If seed is not set a random puzzle will be regenerated.

```
        static void Main(string[] args)
        {
            var w = new Wordsearch(15);

            w.AddWord("EXAMPLE");
            w.AddWord("tester");
            w.AddWord("coolguy");

            w.Case = LetterCase.Upper;
            w.Regenerate();

            w.PrintWordsearch();
            w.PrintSolution();

            string wsStr = w.ToStringWordsearch();
            string solutionStr = w.ToStringSolution();
        }
```