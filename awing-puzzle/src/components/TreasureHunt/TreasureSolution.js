import React from 'react';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  List,
  ListItem,
  ListItemText,
  Divider,
} from '@mui/material';

function TreasureSolution({ solution, puzzle }) {
  // If we don't have solution data, show a message
  if (!solution || !puzzle) {
    return (
      <Box sx={{ p: 3, textAlign: 'center' }}>
        <Typography variant="h6">No solution data available</Typography>
      </Box>
    );
  }

  return (
    <Box>
      <Typography variant="h6" gutterBottom>
        Puzzle Solution
      </Typography>

      <Card sx={{ mb: 4 }}>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6}>
              <Typography variant="body1" color="text.secondary">
                Dimensions
              </Typography>
              <Typography variant="body2">
                Rows (n): {puzzle.n}, Columns (m): {puzzle.m}, Treasure (p): {puzzle.p}
              </Typography>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Typography variant="body1" color="text.secondary">
                Minimum Fuel Required
              </Typography>
              <Typography variant="h5" color="primary" sx={{ fontWeight: 'bold' }}>
                {solution.minFuel !== undefined ? solution.minFuel.toFixed(2) : 'N/A'}
              </Typography>
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      {solution.path && solution.path.length > 0 && (
        <>
          <Typography variant="subtitle1" gutterBottom sx={{ mt: 2 }}>
            Path Taken
          </Typography>
          
          <Paper sx={{ p: 2, mb: 3 }}>
            <List>
              {solution.path.map((step, index) => (
                <React.Fragment key={index}>
                  {index > 0 && <Divider />}
                  <ListItem>
                    <ListItemText
                      primary={`Step ${index + 1}: Island (${step.position.x}, ${step.position.y})`}
                      secondary={
                        <>
                          <Typography component="span" variant="body2" color="text.primary">
                            {index === 0 ? 'Starting position' : `Found key ${step.keyFound} for chest ${step.keyFound + 1}`}
                          </Typography>
                          {index > 0 && (
                            <Typography component="span" variant="body2">
                              {` • Used ${step.fuelUsed.toFixed(2)} fuel`}
                            </Typography>
                          )}
                        </>
                      }
                    />
                  </ListItem>
                </React.Fragment>
              ))}
            </List>
          </Paper>
        </>
      )}

      <Typography variant="subtitle1" gutterBottom>
        Matrix View
      </Typography>
      
      <Paper 
        sx={{ 
          p: 2, 
          overflow: 'auto', 
          maxHeight: '400px'
        }}
      >
        <Box sx={{ display: 'flex', flexDirection: 'column' }}>
          {puzzle.matrix.map((row, rowIndex) => (
            <Box 
              key={rowIndex} 
              sx={{ 
                display: 'flex', 
                flexWrap: 'nowrap'
              }}
            >
              {row.map((cell, colIndex) => {
                // Highlight special positions
                const isStart = rowIndex === 0 && colIndex === 0; // Starting position
                const isTreasure = cell === puzzle.p; // Treasure position
                
                // Determine if this position is in the solution path
                const isInPath = solution.path && solution.path.some(
                  step => step.position.x === rowIndex && step.position.y === colIndex
                );
                
                return (
                  <Box 
                    key={`${rowIndex}-${colIndex}`}
                    sx={{
                      width: '3rem',
                      height: '3rem',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      m: 0.5,
                      border: '1px solid',
                      borderColor: 'divider',
                      borderRadius: '4px',
                      backgroundColor: isTreasure 
                        ? 'gold' 
                        : isStart 
                          ? 'lightblue' 
                          : isInPath 
                            ? 'lightgreen' 
                            : 'white',
                    }}
                  >
                    <Typography>{cell}</Typography>
                  </Box>
                );
              })}
            </Box>
          ))}
        </Box>
      </Paper>
    </Box>
  );
}

export default TreasureSolution;
