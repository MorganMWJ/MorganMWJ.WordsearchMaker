using FluentAssertions;

namespace MorganMWJ.WordsearchMaker.Tests;
public class LanguageExtensionsTests
{
    [TestCase(Language.English, "hello", false)]
    [TestCase(Language.Spanish, "hola", false)]
    [TestCase(Language.French, "bonjour", false)]
    [TestCase(Language.German, "hallo", false)]
    [TestCase(Language.English, "world", false)]
    [TestCase(Language.Spanish, "mundo", false)]
    [TestCase(Language.French, "monde", false)]
    [TestCase(Language.German, "welt", false)]
    [TestCase(Language.English, "invalid word", true)]
    [TestCase(Language.Spanish, "pala123rainvalida", true)]
    [TestCase(Language.French, "motinval=de", true)]
    [TestCase(Language.German, "ungültige=swort", true)]
    public void IsInvalidWordTests(Language language, string word, bool expectedResult)
    {
        // Act
        var result = language.IsInvalidWord(word);

        // Assert
        result.Should().Be(expectedResult);
    }
}
