import { useState } from "react";
import { Box, Typography, Paper, List, ListItem, ListItemText, Alert } from "@mui/material";
import "./WordSearch.css";

type Cell = { row: number; col: number };

type FoundWord = {
  word: string;
  cells: Cell[];
};

type Props = {
  grid: string[][];
  words: string[];
};

export default function WordSearch({ grid, words }: Props) {
  const [isDragging, setIsDragging] = useState(false);
  const [selection, setSelection] = useState<Cell[]>([]);
  const [foundWords, setFoundWords] = useState<FoundWord[]>([]);
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

    if (match && !foundWords.some(fw => fw.word === match)) {
      setFoundWords([...foundWords, { word: match, cells: [...selection] }]);
    }

    setSelection([]);
    setDirection(null);
  }

  function isSelected(row: number, col: number) {
    return selection.some(c => c.row === row && c.col === col);
  }

  function isFoundCell(row: number, col: number) {
    return foundWords.some(fw => fw.cells.some(c => c.row === row && c.col === col));
  }

  return (
    <Box className="wordsearch-container" sx={{ display: 'flex', gap: 4, flexWrap: 'wrap', mt: 3 }}>

      <Alert severity={foundWords.length === words.length ? "success" : "info"} sx={{ width: '100%' }}>
        {foundWords.length === words.length
          ? "Congratulations! You found all the words!"
          : `Words found: ${foundWords.length} / ${words.length}`}
      </Alert>

      <Box className="grid" onMouseLeave={() => setIsDragging(false)}>
        {grid.map((row, r) => (
          <Box key={r} className="row" sx={{ display: 'flex' }}>
            {row.map((letter, c) => (
              <Box
                key={c}
                className={`cell ${isSelected(r, c) ? "selected" : ""} ${isFoundCell(r, c) ? "found" : ""}`}
                onMouseDown={() => handleMouseDown({ row: r, col: c })}
                onMouseEnter={() => handleMouseEnter({ row: r, col: c })}
                onMouseUp={handleMouseUp}
                sx={{
                  width: 40,
                  height: 40,
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  cursor: 'pointer',
                  userSelect: 'none',
                  fontWeight: 'bold',
                  fontSize: '1.1rem',
                  backgroundColor: isFoundCell(r, c) ? 'rgba(33, 150, 243, 0.3)' : 'transparent'
                }}
              >
                {letter}
              </Box>
            ))}
          </Box>
        ))}
      </Box>

      <Paper elevation={2} sx={{ p: 3, minWidth: 200 }}>
        <Typography variant="h5" component="h3" gutterBottom>
          Words
        </Typography>
        <List>
          {words.map(word => {
            const foundWord = foundWords.find(fw => fw.word === word);
            return (
              <ListItem
                key={word}
                sx={{
                  textDecoration: foundWord ? 'line-through' : 'none',
                  color: foundWord ? 'success.main' : 'text.primary'
                }}
              >
                <ListItemText primary={word} />
              </ListItem>
            );
          })}
        </List>
      </Paper>
    </Box>
  );
}
