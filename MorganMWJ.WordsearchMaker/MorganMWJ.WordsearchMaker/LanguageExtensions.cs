using System.Globalization;

namespace MorganMWJ.WordsearchMaker;
internal static class LanguageExtensions
{
    internal static string GetAlphabet(this Language lang, LetterCase c)
    {
        return (c, lang) switch
        {
            (LetterCase.Upper, Language.English) => Alphabets.EnglishUpper,
            (LetterCase.Lower, Language.English) => Alphabets.EnglishLower,
            (LetterCase.Upper, Language.Spanish) => Alphabets.SpanishUpper,
            (LetterCase.Lower, Language.Spanish) => Alphabets.SpanishLower,
            (LetterCase.Upper, Language.French) => Alphabets.FrenchUpper,
            (LetterCase.Lower, Language.French) => Alphabets.FrenchLower,
            (LetterCase.Upper, Language.German) => Alphabets.GermanUpper,
            (LetterCase.Lower, Language.German) => Alphabets.GermanLower,
            (LetterCase.Upper, Language.Italian) => Alphabets.ItalianUpper,
            (LetterCase.Lower, Language.Italian) => Alphabets.ItalianLower,
            (LetterCase.Upper, Language.Portuguese) => Alphabets.PortugueseUpper,
            (LetterCase.Lower, Language.Portuguese) => Alphabets.PortugueseLower,
            (LetterCase.Upper, Language.Dutch) => Alphabets.DutchUpper,
            (LetterCase.Lower, Language.Dutch) => Alphabets.DutchLower,
            (LetterCase.Upper, Language.Swedish) => Alphabets.SwedishUpper,
            (LetterCase.Lower, Language.Swedish) => Alphabets.SwedishLower,
            (LetterCase.Upper, Language.Norwegian) => Alphabets.NorwegianUpper,
            (LetterCase.Lower, Language.Norwegian) => Alphabets.NorwegianLower,
            (LetterCase.Upper, Language.Danish) => Alphabets.DanishUpper,
            (LetterCase.Lower, Language.Danish) => Alphabets.DanishLower,
            (LetterCase.Upper, Language.Finnish) => Alphabets.FinnishUpper,
            (LetterCase.Lower, Language.Finnish) => Alphabets.FinnishLower,
            (LetterCase.Upper, Language.Polish) => Alphabets.PolishUpper,
            (LetterCase.Lower, Language.Polish) => Alphabets.PolishLower,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    internal static bool IsInvalidWord(this Language lang, string word)
    {
        var alphabetUpper = lang.GetAlphabet(LetterCase.Upper);
        var alphabetLower = lang.GetAlphabet(LetterCase.Lower);
        var alphabet = alphabetUpper + alphabetLower;
        return word.Any(ch => !alphabet.Contains(ch));
    }

    internal static CultureInfo GetCultureInfo(this Language lang)
    {
        var cultureInfo = lang switch
        {
            Language.English => new CultureInfo("en-US"),
            Language.Spanish => new CultureInfo("es-ES"),
            Language.French => new CultureInfo("fr-FR"),
            Language.German => new CultureInfo("de-DE"),
            Language.Italian => new CultureInfo("it-IT"),
            Language.Portuguese => new CultureInfo("pt-PT"),
            Language.Dutch => new CultureInfo("nl-NL"),
            Language.Swedish => new CultureInfo("sv-SE"),
            Language.Norwegian => new CultureInfo("nb-NO"),
            Language.Danish => new CultureInfo("da-DK"),
            Language.Finnish => new CultureInfo("fi-FI"),
            Language.Polish => new CultureInfo("pl-PL"),
            _ => throw new ArgumentException($"Unsupported language: {lang}"),
        };
        return cultureInfo;
    }
}
