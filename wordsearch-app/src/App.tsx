import WordSearch from "./WordSearch";

const sampleData = {
  grid: [
    ["C", "A", "T"],
    ["D", "O", "G"],
    ["P", "I", "G"]
  ],
  words: ["CAT", "DOG", "PIG"]
};

function App() {
  return (
    <div style={{ padding: "2rem" }}>
      <h1>Word Search Demo</h1>
      <WordSearch grid={sampleData.grid} words={sampleData.words} />
    </div>
  );
}

export default App;
