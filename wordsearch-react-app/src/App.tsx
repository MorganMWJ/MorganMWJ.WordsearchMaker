import React, { useState } from "react";
import WordSearch from "./WordSearch";
import WordsearchForm from "./WordsearchForm";

type WordSearchData = {
  Grid: string[][];
  Words: string[];
};

function App() {
  const [wordsearchData, setWordsearchData] = useState<WordSearchData  | null>(null);

  return (
    <div style={{ padding: "2rem" }}>
      <h1>Word Search Maker</h1>
      <WordsearchForm onGenerate={setWordsearchData} />
      
      {wordsearchData ? (
        <WordSearch grid={wordsearchData.Grid} words={wordsearchData.Words} />
      ) : (
        <div style={{ color: "#888" }}>Fill out the form and generate a wordsearch!</div>
      )}
    </div>
  );
}

export default App;
