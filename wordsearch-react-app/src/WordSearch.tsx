import { useState } from "react";
import "./WordSearch.css";

type Cell = { row: number; col: number };

type Props = {
  grid: string[][];
  words: string[];
};

export default function WordSearch({ grid, words }: Props) {
  const [isDragging, setIsDragging] = useState(false);
  const [selection, setSelection] = useState<Cell[]>([]);
  const [foundWords, setFoundWords] = useState<string[]>([]);
  const [direction, setDirection] = useState<{ dx: number; dy: number } | null>(null);

  // --- Helper to compute direction ---
  function computeDirection(start: Cell, end: Cell) {
    const dx = end.col - start.col;
    const dy = end.row - start.row;

    const stepX = dx === 0 ? 0 : dx / Math.abs(dx);
    const stepY = dy === 0 ? 0 : dy / Math.abs(dy);

    // Allow only straight or perfect diagonal lines
    if (stepX !== 0 && stepY !== 0 && Math.abs(dx) !== Math.abs(dy)) return null;

    return { dx: stepX, dy: stepY };
  }

  // --- Generate cells along the direction ---
  function generateCells(start: Cell, end: Cell, dir: { dx: number; dy: number }) {
    const cells: Cell[] = [];
    let row = start.row;
    let col = start.col;

    while (true) {
      cells.push({ row, col });
      if (row === end.row && col === end.col) break;
      row += dir.dy;
      col += dir.dx;
    }

    return cells;
  }

  // --- Event handlers ---
  function handleMouseDown(cell: Cell) {
    setIsDragging(true);
    setSelection([cell]);
    setDirection(null);
  }

  function handleMouseEnter(cell: Cell) {
    if (!isDragging) return;
    const start = selection[0];

    // if direction from start straight line or diagonal
    // And must be the same direction as before (if set)
    // then add current cell to selection
    const dir = computeDirection(start, cell);
    var isStraight = dir !== null;
    if (isStraight && (direction === null || (dir!.dx === direction.dx && dir!.dy === direction.dy))) {
      var newSelection = generateCells(start, cell, dir!);
      setSelection(newSelection);
      setDirection(dir);
      return;
    }
    setDirection(dir);
    // I am not sure how this works so well yey- investigate later
  }

  function handleMouseUp() {
    setIsDragging(false);
    if (!selection.length) return;

    const selectedWord = selection.map(c => grid[c.row][c.col]).join("");
    const reversed = selectedWord.split("").reverse().join("");

    const match = words.find(
      w => w === selectedWord || w === reversed
    );

    if (match && !foundWords.includes(match)) {
      setFoundWords([...foundWords, match]);
    }

    setSelection([]);
    setDirection(null);
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
