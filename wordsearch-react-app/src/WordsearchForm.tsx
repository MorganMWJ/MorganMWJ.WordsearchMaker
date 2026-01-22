import React, { useState } from 'react';
import {
  Box,
  TextField,
  Slider,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Button,
  Alert,
  Typography
} from '@mui/material';

interface WordsearchFormProps {
  onGenerate: (data: any) => void;
}

const WordsearchForm: React.FC<WordsearchFormProps> = ({ onGenerate }) => {
  const [words, setWords] = useState('');
  const [gridSize, setGridSize] = useState(20);
  const [letterCase, setLetterCase] = useState('lower');
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
      const data = await generateWordsearch({ gridSize, wordList, letterCase });
      onGenerate(data);
    } catch (err: any) {
      setError(err.message || 'Unknown error');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ mb: 4 }}>
      <Box sx={{ mb: 3 }}>
        <TextField
          label="Words (comma or newline separated)"
          value={words}
          onChange={e => setWords(e.target.value)}
          multiline
          rows={6}
          fullWidth
          placeholder="cat, dog, pig, ..."
          variant="outlined"
        />
      </Box>

      <Box sx={{ mb: 3 }}>
        <Typography gutterBottom>
          Wordsearch Size: {gridSize}
        </Typography>
        <Slider
          value={gridSize}
          onChange={(_, value) => setGridSize(value as number)}
          min={8}
          max={50}
          valueLabelDisplay="auto"
          marks={[
            { value: 8, label: '8' },
            { value: 50, label: '50' }
          ]}
        />
      </Box>

      <Box sx={{ mb: 3 }}>
        <FormControl fullWidth>
          <InputLabel id="case-select-label">Wordsearch Case</InputLabel>
          <Select
            labelId="case-select-label"
            value={letterCase}
            label="Wordsearch Case"
            onChange={e => setLetterCase(e.target.value)}
          >
            <MenuItem value="lower">Lower</MenuItem>
            <MenuItem value="upper">Upper</MenuItem>
          </Select>
        </FormControl>
      </Box>

      <Button
        type="submit"
        variant="contained"
        size="large"
        disabled={loading}
        fullWidth
      >
        {loading ? 'Generating...' : 'Generate Wordsearch'}
      </Button>

      {error && (
        <Alert severity="error" sx={{ mt: 2 }}>
          {error}
        </Alert>
      )}
    </Box>
  );
};

export default WordsearchForm;
