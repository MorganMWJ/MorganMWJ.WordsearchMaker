const API_URL = 'https://wordsearch-generator-api-f9excqg7bhehbjam.uksouth-01.azurewebsites.net/api/generatewordsearch';

export async function generateWordsearch({ gridSize, wordList, letterCase }: { gridSize: number; wordList: string[]; letterCase: string }) {
  const generationSeed = Math.floor(Math.random() * 1000000);
  const body = {
    gridSize,
    wordList,
    letterCase,
    generationSeed,
  };
  const response = await fetch(API_URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(body),
  });
  if (!response.ok) {
    const errorText = await response.text();
    try {
      const errorJson = JSON.parse(errorText);
      throw new Error(errorJson.error || `API error: ${response.status}`);
    } catch (parseError) {
      // If JSON parsing fails, throw the raw error text
      throw new Error(`API error: ${response.status}\n${errorText}`);
    }
  }
  return await response.json();
}
