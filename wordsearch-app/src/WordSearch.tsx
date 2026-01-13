import { useState } from "react";
import "./WordSearch.css";

type Cell = {
  row: number;
  col: number;
};

type Props = {
  grid: string[][];
  words: string[];
};

export default function WordSearch({ grid, words }: Props) {
  const [isDragging, setIsDragging] = useState(false);
  const [selection, setSelection] = useState<Cell[]>([]);
  const [foundWords, setFoundWords] = useState<string[]>([]);

  function handleMouseDown(cell: Cell) {
    setIsDragging(true);
    setSelection([cell]);
  }

  function handleMouseEnter(cell: Cell) {
    if (!isDragging) return;
    const last = selection[selection.length - 1];
    if (last.row === cell.row && last.col === cell.col) return;
    setSelection([...selection, cell]);
  }

  function handleMouseUp() {
    setIsDragging(false);
    const selectedWord = selection.map(c => grid[c.row][c.col]).join("");
    const reversed = selectedWord.split("").reverse().join("");

    const match = words.find(
      w => w.toUpperCase() === selectedWord || w.toUpperCase() === reversed
    );

    if (match && !foundWords.includes(match)) {
      setFoundWords([...foundWords, match]);
    }

    setSelection([]);
  }

  function isSelected(row: number, col: number) {
    return selection.some(c => c.row === row && c.col === col);
  }

  return (
    <div className="wordsearch-container">
      <div className="grid" onMouseLeave={() => setIsDragging(false)}>
        {grid.map((row, r) => (
          <div key={r} className="row">
            {row.map((letter, c) => (
              <div
                key={c}
                className={`cell ${isSelected(r, c) ? "selected" : ""}`}
                onMouseDown={() => handleMouseDown({ row: r, col: c })}
                onMouseEnter={() => handleMouseEnter({ row: r, col: c })}
                onMouseUp={handleMouseUp}
              >
                {letter}
              </div>
            ))}
          </div>
        ))}
      </div>

      <div className="word-list">
        <h3>Words</h3>
        <ul>
          {words.map(word => (
            <li key={word} className={foundWords.includes(word) ? "found" : ""}>
              {word}
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}
