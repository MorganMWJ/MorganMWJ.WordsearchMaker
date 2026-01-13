using FluentAssertions;

namespace MorganMWJ.WordsearchMaker.Tests;

public class WordsearchTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Build_Wordsearch_using_AddWord_method()
    {
        // Arrange
        var ws = new Wordsearch(30);
        ws.AddWord("hello");
        ws.AddWord("world");

        // Act
        ws.Regenerate();

        // Assert
        ws._wordsPositions.Should().HaveCount(2);
        ws._wordsPositions.Keys.Should().Contain("hello");
        ws._wordsPositions.Keys.Should().Contain("world");

        AssertWordsInPositions(ws);
    }

    [Test]
    public void Build_Wordsearch_using_AddWords_method()
    {
        // Arrange
        var ws = new Wordsearch(30);
        ws.AddWords(["hello", "world"]);

        // Act
        ws.Regenerate();

        // Assert
        ws._wordsPositions.Should().HaveCount(2);
        ws._wordsPositions.Keys.Should().Contain("hello");
        ws._wordsPositions.Keys.Should().Contain("world");

        AssertWordsInPositions(ws);
    }

    [Test]
    public void Build_Wordsearch_using_AddWords_method_multi_call_regenerate()
    {
        // Arrange
        var ws = new Wordsearch(30);
        ws.AddWords(["hello", "world"]);

        // Act
        ws.Regenerate();
        ws.Regenerate();
        ws.Regenerate();

        // Assert
        ws._wordsPositions.Should().HaveCount(2);
        ws._wordsPositions.Keys.Should().Contain("hello");
        ws._wordsPositions.Keys.Should().Contain("world");

        AssertWordsInPositions(ws);
    }

    [Test]
    public void Build_Wordsearch_using_nonprimary_Constructor_()
    {
        // Arrange
        var ws = new Wordsearch(["hello", "world"], 30);
        ws.AddWords(["beauty", "you", "are"]);

        // Act
        ws.Regenerate();

        // Assert
        ws._wordsPositions.Should().HaveCount(5);
        ws._wordsPositions.Keys.Should().Contain("hello");
        ws._wordsPositions.Keys.Should().Contain("world");
        ws._wordsPositions.Keys.Should().Contain("beauty");
        ws._wordsPositions.Keys.Should().Contain("you");
        ws._wordsPositions.Keys.Should().Contain("are");

        AssertWordsInPositions(ws);
    }

    [Test]
    public void Building_Wordsearch_using_duplicate_words_throws_ArgumentException()
    {
        Action act = () => new Wordsearch(["hello", "world", "hello"], 30);
        act.Should().Throw<ArgumentException>()
            .WithMessage("An item with the same key has already been added*");
    }

    [Test]
    public void Building_Wordsearch_using_duplicate_words_throws_ArgumentException_2()
    {
        Action act = () =>
        {
            var ws = new Wordsearch(["hello", "world"], 30);
            ws.AddWord("hello");
        };

        act.Should().Throw<ArgumentException>()
        .WithMessage("An item with the same key has already been added*");
    }

    [Test]
    public void Building_Wordsearch_using_duplicate_words_throws_ArgumentException_3()
    {
        Action act = () =>
        {
            var ws = new Wordsearch(30);
            ws.AddWords(["hello", "world", "hello"]);
        };

        act.Should().Throw<ArgumentException>()
        .WithMessage("An item with the same key has already been added*");
    }

    [Test]
    public void Building_Wordsearch_via_constructor_with_word_too_long_throws_ArgumentException()
    {
        Action act = () => new Wordsearch(["thiswordiswaytoolongtobeincluded"], 10);
        act.Should().Throw<ArgumentException>()
            .WithMessage("The word 'thiswordiswaytoolongtobeincluded' is too long for the grid size 10x10.");
    }

    [Test]
    public void Building_Wordsearch_via_AddWords_with_word_too_long_throws_ArgumentException()
    {
        Action act = () =>
        {
            var ws = new Wordsearch(10);
            ws.AddWords(["thiswordiswaytoolongtobeincluded", "good"]);
        };

        act.Should().Throw<ArgumentException>()
            .WithMessage("The word 'thiswordiswaytoolongtobeincluded' is too long for the grid size 10x10.");
    }

    [Test]
    public void Building_Wordsearch_via_AddWord_with_word_too_long_throws_ArgumentException()
    {
        Action act = () =>
        {
            var ws = new Wordsearch(10);
            ws.AddWord("good");
            ws.AddWord("fine");
            ws.AddWord("thiswordiswaytoolongtobeincluded");
        };

        act.Should().Throw<ArgumentException>()
            .WithMessage("The word 'thiswordiswaytoolongtobeincluded' is too long for the grid size 10x10.");
    }

    [Test]
    public void Building_wordsearch_with_invalid_grid_size_throws_ArgumentException()
    {
        Action act1 = () => new Wordsearch(7);
        act1.Should().Throw<ArgumentOutOfRangeException>();
        Action act2 = () => new Wordsearch(71);
        act2.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void Building_wordsearch_with_no_words_is_allowed()
    {
        // Arrange
        var ws = new Wordsearch(20);

        // Act
        ws.Regenerate();

        // Assert
        ws.Words.Should().BeEmpty();
        ws._wordsPositions.Should().BeEmpty();
        var result = ws.ToStringWordsearch();
        result.All(c => char.IsLetter(c) || c=='\n' || c=='\r').Should().BeTrue();
    }

    [Test]
    public void Uppercase_Wordsearch_is_all_uppercase()
    {
        // Arrange
        var ws = new Wordsearch(30);
        ws.AddWord("hello");
        ws.AddWord("starshine");
        ws.AddWord("THE");
        ws.AddWord("world");
        ws.AddWord("SAYs");

        ws.Case = LetterCase.Upper;

        // Act
        ws.Regenerate();

        // Assert
        ws._wordsPositions.Should().HaveCount(5);
        ws._wordsPositions.Keys.Should().Contain("HELLO");
        ws._wordsPositions.Keys.Should().Contain("STARSHINE");
        ws._wordsPositions.Keys.Should().Contain("THE");
        ws._wordsPositions.Keys.Should().Contain("WORLD");
        ws._wordsPositions.Keys.Should().Contain("SAYS");

        var result = ws.ToStringWordsearch();
        result.All(c => char.IsUpper(c) || c=='\n' || c=='\r').Should().BeTrue();
    }

    [Test]
    public void Lowercase_Wordsearch_is_all_lowercase()
    {
        // Arrange
        var ws = new Wordsearch(30);
        ws.AddWord("hello");
        ws.AddWord("starshine");
        ws.AddWord("THE");
        ws.AddWord("world");
        ws.AddWord("SAYs");

        // Act
        ws.Regenerate();

        // Assert
        ws._wordsPositions.Should().HaveCount(5);
        ws._wordsPositions.Keys.Should().Contain("hello");
        ws._wordsPositions.Keys.Should().Contain("starshine");
        ws._wordsPositions.Keys.Should().Contain("the");
        ws._wordsPositions.Keys.Should().Contain("world");
        ws._wordsPositions.Keys.Should().Contain("says");

        var result = ws.ToStringWordsearch();
        result.All(c => char.IsLower(c) || c=='\n' || c=='\r').Should().BeTrue();
    }

    [Test]
    public void Same_Seed_Produces_Same_Wordsearch()
    {
        // Arrange
        var ws1 = new Wordsearch(30, seed: 12345);
        ws1.AddWords(["hello", "world", "test", "wordsearch", "example"]);
        ws1.Case = LetterCase.Upper;
        ws1.AllowedDirection = WordDirection.Horizontal | WordDirection.Vertical;
        ws1.Regenerate();

        var ws2 = new Wordsearch(30, seed: 12345);
        ws2.AddWords(["hello", "world", "test", "wordsearch", "example"]);
        ws2.Case = LetterCase.Upper;
        ws2.AllowedDirection = WordDirection.Horizontal | WordDirection.Vertical;

        // Act
        ws2.Regenerate();

        // Assert
        ws1.ToStringWordsearch().Should().Be(ws2.ToStringWordsearch());
        ws1.ToStringSolution().Should().Be(ws2.ToStringSolution());
    }

    [TestCase("hello2")]
    [TestCase("world!")]
    [TestCase("wor ld!")]
    [TestCase("world  ")]
    public void Words_with_non_alpha_characters_throw_ArgumentException(string word)
    {
        // Arrange
        var ws = new Wordsearch(20);

        // Act
        Action act = () => ws.AddWord(word);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Input word '{word}' must contain only contain valid English alphabet characters.");
    }

    [Test]
    public void Output_methods_should_throw_InvalidOperationException_when_Regenerate_not_called()
    {
        // Arrange
        var ws = new Wordsearch(20);
        ws.AddWords(["hello", "world"]);

        // Act
        Action act1 = () => ws.ToStringWordsearch();
        Action act2 = () => ws.ToStringSolution();
        Action act3 = () => ws.GetWordsearchLines();
        Action act4 = () => ws.GetSolutionLines();

        // Assert
        var expectedMessage = "The wordsearch grid has not been generated yet. Please call Regenerate() before calling this method.";
        act1.Should().Throw<InvalidOperationException>()
            .WithMessage(expectedMessage);
        act2.Should().Throw<InvalidOperationException>()
            .WithMessage(expectedMessage);
        act3.Should().Throw<InvalidOperationException>()
            .WithMessage(expectedMessage);
        act4.Should().Throw<InvalidOperationException>()
            .WithMessage(expectedMessage);
    }

    [Test]
    public void Changing_Case_property_updates_word_cases()
    {
        // Arrange
        var ws = new Wordsearch(30);
        ws.AddWords(["hello", "world"]);
        // Act
        ws.Regenerate();
        // Assert initial case (lowercase)
        ws._wordsPositions.Keys.Should().Contain("hello");
        ws._wordsPositions.Keys.Should().Contain("world");
        // Change to uppercase and regenerate
        ws.Case = LetterCase.Upper;
        ws.Regenerate();
        // Assert updated case (uppercase)
        ws._wordsPositions.Keys.Should().Contain("HELLO");
        ws._wordsPositions.Keys.Should().Contain("WORLD");
    }

    [Test]
    public void Changing_Language_property_clears_words()
    {
        // Arrange
        var ws = new Wordsearch(30);
        ws.AddWords(["hello", "world"]);

        // Act
        ws.Regenerate();
        // Assert initial language (English)
        ws.Words.Should().Contain("hello");
        ws.Words.Should().Contain("world");
        // Change to French
        ws.Language = Language.French;

        // Assert updated language (French)
        ws.Words.Should().BeEmpty();
    }

    private void AssertWordsInPositions(Wordsearch ws)
    {
        foreach (var word in ws.Words)
        {
            int index = 0;
            foreach (var pos in ws._wordsPositions[word])
            {
                var gridStr = ws.ToStringWordsearch();
                var solStr = ws.ToStringSolution();
                ws._grid[pos.X, pos.Y].Should().Be(word[index]);
                index++;
            }
        }
    }
}