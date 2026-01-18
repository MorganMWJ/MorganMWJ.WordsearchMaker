
namespace MorganMWJ.WordsearchMaker.Api;

public interface IWordsearchFactory
{
    Wordsearch Create(WordSearchRequest request);
}