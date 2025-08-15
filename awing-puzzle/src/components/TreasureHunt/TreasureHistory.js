import React from 'react';
import {
  Box,
  Typography,
  Paper,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Divider,
  Card,
  CardContent,
  Grid,
  Tooltip,
} from '@mui/material';
import {
  History as HistoryIcon,
  PlayArrow as PlayIcon,
} from '@mui/icons-material';

function formatDate(dateString) {
  const options = { 
    year: 'numeric', 
    month: 'short', 
    day: 'numeric', 
    hour: '2-digit', 
    minute: '2-digit' 
  };
  return new Date(dateString).toLocaleDateString(undefined, options);
}

function TreasureHistory({ puzzles = [], onLoad }) {
  if (!puzzles || puzzles.length === 0) {
    return (
      <Box sx={{ p: 2, textAlign: 'center' }}>
        <Typography variant="subtitle1">
          No saved puzzles found. Solve a puzzle to see it here!
        </Typography>
      </Box>
    );
  }

  return (
    <Box>
      <Typography variant="h6" gutterBottom>
        Saved Puzzles
      </Typography>

      <Paper sx={{ width: '100%', mb: 2 }}>
        <List>
          {puzzles.map((puzzle, index) => (
            <React.Fragment key={puzzle.id || index}>
              {index > 0 && <Divider />}
              <ListItem>
                <ListItemText
                  primary={`Puzzle ${index + 1} (${puzzle.n}x${puzzle.m}, p=${puzzle.p})`}
                  secondary={puzzle.createdAt ? `Created: ${formatDate(puzzle.createdAt)}` : 'No date'}
                />
              </ListItem>
              <Box sx={{ px: 2, pb: 2 }}>
                <Card variant="outlined">
                  <CardContent sx={{ py: 1 }}>
                    <Grid container spacing={2}>
                      <Grid item xs={12}>
                        <Box sx={{ display: 'flex', flexDirection: 'column', maxHeight: '150px', overflow: 'auto' }}>
                          {puzzle.matrix.map((row, rowIndex) => (
                            <Box key={rowIndex} sx={{ display: 'flex', justifyContent: 'center' }}>
                              {row.map((cell, colIndex) => (
                                <Box 
                                  key={`${rowIndex}-${colIndex}`}
                                  sx={{
                                    width: '1.5rem',
                                    height: '1.5rem',
                                    display: 'flex',
                                    alignItems: 'center',
                                    justifyContent: 'center',
                                    m: 0.25,
                                    border: '1px solid',
                                    borderColor: 'divider',
                                    borderRadius: '2px',
                                    fontSize: '0.75rem',
                                    backgroundColor: cell === puzzle.p ? 'gold' : 'transparent',
                                  }}
                                >
                                  {cell}
                                </Box>
                              ))}
                            </Box>
                          ))}
                        </Box>
                      </Grid>
                      {puzzle.result !== undefined && (
                        <Grid item xs={12}>
                          <Typography variant="body2">
                            Result: <strong>{puzzle.result}</strong>
                          </Typography>
                        </Grid>
                      )}
                    </Grid>
                  </CardContent>
                </Card>
              </Box>
            </React.Fragment>
          ))}
        </List>
      </Paper>
    </Box>
  );
}

export default TreasureHistory;
