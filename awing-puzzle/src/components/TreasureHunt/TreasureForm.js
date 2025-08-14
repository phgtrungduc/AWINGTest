import React, { useState, useEffect } from 'react';
import {
  Box,
  TextField,
  Button,
  Grid,
  Typography,
  Paper,
  FormHelperText,
  FormControl,
} from '@mui/material';

function TreasureForm({ onSolve, currentPuzzle }) {
  const [dimensions, setDimensions] = useState({
    n: '',
    m: '',
    p: ''
  });
  const [matrix, setMatrix] = useState([]);
  const [errors, setErrors] = useState({
    n: '',
    m: '',
    p: '',
    matrix: []
  });

  // Initialize matrix when dimensions change
  useEffect(() => {
    const { n, m } = dimensions;
    if (n > 0 && m > 0 && n <= 500 && m <= 500) {
      const newMatrix = Array(parseInt(n)).fill().map(() => Array(parseInt(m)).fill(''));
      setMatrix(newMatrix);
      
      // Initialize matrix errors array
      const newErrors = Array(parseInt(n)).fill().map(() => Array(parseInt(m)).fill(''));
      setErrors(prev => ({ ...prev, matrix: newErrors }));
    }
  }, [dimensions.n, dimensions.m]);

  // Load current puzzle if available
  useEffect(() => {
    if (currentPuzzle) {
      setDimensions({
        n: currentPuzzle.n,
        m: currentPuzzle.m,
        p: currentPuzzle.p
      });
      setMatrix(currentPuzzle.matrix);
    }
  }, [currentPuzzle]);

  const validateDimensions = () => {
    const newErrors = { ...errors };
    let isValid = true;

    // Validate n
    if (!dimensions.n) {
      newErrors.n = 'Number of rows is required';
      isValid = false;
    } else if (isNaN(dimensions.n) || dimensions.n <= 0) {
      newErrors.n = 'Must be a positive number';
      isValid = false;
    } else if (dimensions.n > 500) {
      newErrors.n = 'Maximum value is 500';
      isValid = false;
    } else {
      newErrors.n = '';
    }

    // Validate m
    if (!dimensions.m) {
      newErrors.m = 'Number of columns is required';
      isValid = false;
    } else if (isNaN(dimensions.m) || dimensions.m <= 0) {
      newErrors.m = 'Must be a positive number';
      isValid = false;
    } else if (dimensions.m > 500) {
      newErrors.m = 'Maximum value is 500';
      isValid = false;
    } else {
      newErrors.m = '';
    }

    // Validate p
    if (!dimensions.p) {
      newErrors.p = 'Treasure number is required';
      isValid = false;
    } else if (isNaN(dimensions.p) || dimensions.p <= 0) {
      newErrors.p = 'Must be a positive number';
      isValid = false;
    } else if (dimensions.p > dimensions.n * dimensions.m) {
      newErrors.p = `Maximum value is n*m (${dimensions.n * dimensions.m})`;
      isValid = false;
    } else {
      newErrors.p = '';
    }

    setErrors(newErrors);
    return isValid;
  };

  const validateMatrix = () => {
    const n = parseInt(dimensions.n);
    const m = parseInt(dimensions.m);
    const p = parseInt(dimensions.p);
    const newErrors = { ...errors };
    let isValid = true;
    let pFound = false;

    // Initialize matrix errors
    newErrors.matrix = Array(n).fill().map(() => Array(m).fill(''));

    // Check each cell
    for (let i = 0; i < n; i++) {
      for (let j = 0; j < m; j++) {
        const value = matrix[i][j];
        
        // Check if the value is empty
        if (value === '' || value === null) {
          newErrors.matrix[i][j] = 'Required';
          isValid = false;
          continue;
        }
        
        const numValue = parseInt(value);
        
        // Check if the value is a number
        if (isNaN(numValue)) {
          newErrors.matrix[i][j] = 'Must be a number';
          isValid = false;
          continue;
        }
        
        // Check if the value is in the valid range
        if (numValue < 1 || numValue > p) {
          newErrors.matrix[i][j] = `Must be 1-${p}`;
          isValid = false;
          continue;
        }
        
        // Check if this is the treasure value
        if (numValue === p) {
          pFound = true;
        }
        
        // No error for this cell
        newErrors.matrix[i][j] = '';
      }
    }

    // Check if we found the treasure value
    if (!pFound) {
      isValid = false;
      newErrors.p = `Matrix must contain exactly one cell with value ${p}`;
    }

    setErrors(newErrors);
    return isValid;
  };

  const handleDimensionChange = (e) => {
    const { name, value } = e.target;
    
    // Only allow positive integers up to 500
    if (value === '' || (/^[1-9]\d*$/.test(value) && parseInt(value) <= 500)) {
      setDimensions(prev => ({ ...prev, [name]: value }));
    }
  };

  const handleMatrixChange = (row, col, value) => {
    // Only allow positive integers up to the value of p
    if (value === '' || (/^[1-9]\d*$/.test(value) && parseInt(value) <= dimensions.p)) {
      const newMatrix = [...matrix];
      newMatrix[row][col] = value;
      setMatrix(newMatrix);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    // Validate dimensions and matrix
    const dimensionsValid = validateDimensions();
    if (!dimensionsValid) return;
    
    const matrixValid = validateMatrix();
    if (!matrixValid) return;

    // Convert to numeric values
    const numericMatrix = matrix.map(row => row.map(cell => parseInt(cell)));
    
    // Call the onSolve callback with the puzzle data
    onSolve({
      n: parseInt(dimensions.n),
      m: parseInt(dimensions.m),
      p: parseInt(dimensions.p),
      matrix: numericMatrix
    });
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2 }}>
      <Typography variant="h6" gutterBottom>
        Enter Puzzle Dimensions
      </Typography>
      
      <Grid container spacing={3} sx={{ mb: 4 }}>
        <Grid item xs={12} sm={4}>
          <FormControl fullWidth error={!!errors.n}>
            <TextField
              label="Number of rows (n)"
              name="n"
              value={dimensions.n}
              onChange={handleDimensionChange}
              variant="outlined"
              fullWidth
              error={!!errors.n}
              helperText={errors.n || 'Enter a value between 1 and 500'}
              inputProps={{ inputMode: 'numeric', pattern: '[1-9][0-9]*' }}
            />
          </FormControl>
        </Grid>
        
        <Grid item xs={12} sm={4}>
          <FormControl fullWidth error={!!errors.m}>
            <TextField
              label="Number of columns (m)"
              name="m"
              value={dimensions.m}
              onChange={handleDimensionChange}
              variant="outlined"
              fullWidth
              error={!!errors.m}
              helperText={errors.m || 'Enter a value between 1 and 500'}
              inputProps={{ inputMode: 'numeric', pattern: '[1-9][0-9]*' }}
            />
          </FormControl>
        </Grid>
        
        <Grid item xs={12} sm={4}>
          <FormControl fullWidth error={!!errors.p}>
            <TextField
              label="Treasure number (p)"
              name="p"
              value={dimensions.p}
              onChange={handleDimensionChange}
              variant="outlined"
              fullWidth
              error={!!errors.p}
              helperText={errors.p || 'Enter a value between 1 and n*m'}
              inputProps={{ inputMode: 'numeric', pattern: '[1-9][0-9]*' }}
            />
          </FormControl>
        </Grid>
      </Grid>

      {matrix.length > 0 && (
        <>
          <Typography variant="h6" gutterBottom>
            Enter Matrix Values
          </Typography>
          
          <Paper 
            sx={{ 
              p: 2, 
              overflow: 'auto', 
              maxHeight: '400px',
              '& .MuiTextField-root': { m: 0.5, width: '4rem' } 
            }}
          >
            <Box sx={{ display: 'flex', flexDirection: 'column' }}>
              {matrix.map((row, rowIndex) => (
                <Box key={rowIndex} sx={{ display: 'flex', flexWrap: 'nowrap' }}>
                  {row.map((cell, colIndex) => (
                    <TextField
                      key={`${rowIndex}-${colIndex}`}
                      size="small"
                      value={cell}
                      onChange={(e) => handleMatrixChange(rowIndex, colIndex, e.target.value)}
                      variant="outlined"
                      error={!!errors.matrix?.[rowIndex]?.[colIndex]}
                      helperText={errors.matrix?.[rowIndex]?.[colIndex] || ''}
                      inputProps={{ 
                        style: { textAlign: 'center', padding: '8px 4px' },
                        inputMode: 'numeric'
                      }}
                    />
                  ))}
                </Box>
              ))}
            </Box>
          </Paper>
        </>
      )}

      <Box sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <Button 
          type="submit" 
          variant="contained" 
          color="primary" 
          size="large"
          disabled={!dimensions.n || !dimensions.m || !dimensions.p}
        >
          Solve Puzzle
        </Button>
      </Box>
    </Box>
  );
}

export default TreasureForm;
