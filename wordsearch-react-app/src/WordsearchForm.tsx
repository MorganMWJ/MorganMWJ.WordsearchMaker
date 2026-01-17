import React, { useState } from 'react';

interface WordsearchFormProps {
  onGenerate: (data: any) => void;
}

const WordsearchForm: React.FC<WordsearchFormProps> = ({ onGenerate }) => {
  const [words, setWords] = useState('');
  const [gridSize, setGridSize] = useState(20);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    const wordList = words
      .split(/[\n,]/)
      .map(w => w.trim())
      .filter(w => w.length > 0);
    if (wordList.length === 0) {
      setError('Please enter at least one word.');
      setLoading(false);
      return;
    }
    try {
      const { generateWordsearch } = await import('./api');
      const data = await generateWordsearch({ gridSize, wordList });
      onGenerate(data);
    } catch (err: any) {
      setError(err.message || 'Unknown error');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: '2rem' }}>
      <div style={{ marginBottom: '1rem' }}>
        <label>
          Words (comma or newline separated):<br />
          <textarea
            value={words}
            onChange={e => setWords(e.target.value)}
            rows={6}
            cols={40}
            style={{ marginTop: '0.5rem' }}
            placeholder="cat, dog, pig, ..."
          />
        </label>
      </div>
      <div style={{ marginBottom: '1rem' }}>
        <label>
          Wordsearch Size: {gridSize}
          <input
            type="range"
            min={8}
            max={50}
            value={gridSize}
            onChange={e => setGridSize(Number(e.target.value))}
            style={{ marginLeft: '1rem' }}
          />
        </label>
      </div>
      <button type="submit" disabled={loading}>
        {loading ? 'Generating...' : 'Generate Wordsearch'}
      </button>
      {error && <div style={{ color: 'red', marginTop: '1rem' }}>{error}</div>}
    </form>
  );
};

export default WordsearchForm;
