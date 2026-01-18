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
    throw new Error('API error: ' + response.status);
  }
  return await response.json();
}
