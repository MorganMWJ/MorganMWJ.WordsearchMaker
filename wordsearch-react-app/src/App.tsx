import { useState } from "react";
import { Container, Typography, Box } from "@mui/material";
import WordSearch from "./WordSearch";
import WordsearchForm from "./WordsearchForm";

type WordSearchData = {
  Grid: string[][];
  Words: string[];
};

function App() {
  const [wordsearchData, setWordsearchData] = useState<WordSearchData  | null>(null);

  return (
    <Container maxWidth="lg" sx={{ py: 4, display: 'flex', flexDirection: 'column', alignItems: 'center', margin: '0 auto' }}>
      <Typography variant="h3" component="h1" gutterBottom textAlign="center">
        Word Search Maker
      </Typography>
      <Box sx={{ width: '100%', maxWidth: 600 }}>
        <WordsearchForm onGenerate={setWordsearchData} />
      </Box>
      
      {wordsearchData ? (
        <WordSearch grid={wordsearchData.Grid} words={wordsearchData.Words} />
      ) : (
        <Box sx={{ color: "text.secondary", mt: 2, textAlign: 'center' }}>
          Fill out the form and generate a wordsearch!
        </Box>
      )}
    </Container>
  );
}

export default App;
