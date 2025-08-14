import React from 'react';
import { ThemeProvider, createTheme, CssBaseline, Container, Box } from '@mui/material';
import TreasureHunt from './components/TreasureHunt/TreasureHunt';
import './App.css';

// Create a theme instance
const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
    background: {
      default: '#f5f5f5',
    },
  },
  typography: {
    fontFamily: [
      'Roboto',
      'Arial',
      'sans-serif',
    ].join(','),
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Box 
        sx={{
          minHeight: '100vh',
          bgcolor: 'background.default',
          pt: 4,
          pb: 6,
        }}
      >
        <Container maxWidth="lg">
          <TreasureHunt />
        </Container>
      </Box>
    </ThemeProvider>
  );
}

export default App;