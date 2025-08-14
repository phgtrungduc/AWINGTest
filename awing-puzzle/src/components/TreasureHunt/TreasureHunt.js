import React, { useState, useEffect } from 'react';
import {
  Container,
  Typography,
  Paper,
  Box,
  Tabs,
  Tab,
  CircularProgress,
  Alert,
} from '@mui/material';
import TreasureForm from './TreasureForm';
import TreasureSolution from './TreasureSolution';
import TreasureHistory from './TreasureHistory';
import apiService from '../../services/api';

function TabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ p: 3 }}>{children}</Box>}
    </div>
  );
}

function a11yProps(index) {
  return {
    id: `simple-tab-${index}`,
    'aria-controls': `simple-tabpanel-${index}`,
  };
}

function TreasureHunt() {
  const [tabValue, setTabValue] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [solution, setSolution] = useState(null);
  const [savedPuzzles, setSavedPuzzles] = useState([]);
  const [selectedPuzzleId, setSelectedPuzzleId] = useState(null);
  const [currentPuzzle, setCurrentPuzzle] = useState(null);

  // Fetch saved puzzles on component mount
  useEffect(() => {
    const fetchPuzzles = async () => {
      try {
        const puzzles = await apiService.getAllPuzzles();
        setSavedPuzzles(puzzles);
      } catch (error) {
        setError('Failed to load saved puzzles. Please try again later.');
      }
    };
    fetchPuzzles();
  }, []);

  const handleTabChange = (event, newValue) => {
    setTabValue(newValue);
  };

  const handleSolvePuzzle = async (puzzleData) => {
    setLoading(true);
    setError(null);
    try {
      // Save the puzzle first
      const savedPuzzle = await apiService.savePuzzle(puzzleData);
      
      // Then solve it
      const result = await apiService.solvePuzzle(puzzleData);
      
      setSolution(result);
      setCurrentPuzzle(savedPuzzle);
      
      // Refresh the list of saved puzzles
      const puzzles = await apiService.getAllPuzzles();
      setSavedPuzzles(puzzles);
      
      // Switch to the solution tab
      setTabValue(1);
    } catch (error) {
      setError('Error solving the puzzle. Please check your input and try again.');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const handleLoadPuzzle = async (id) => {
    setLoading(true);
    setError(null);
    try {
      const puzzle = await apiService.getPuzzle(id);
      setCurrentPuzzle(puzzle);
      
      // Solve the loaded puzzle
      const result = await apiService.solvePuzzle(puzzle);
      setSolution(result);
      
      // Switch to the solution tab
      setTabValue(1);
    } catch (error) {
      setError('Error loading the puzzle. Please try again.');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom align="center">
        Treasure Hunt Puzzle
      </Typography>
      
      <Paper sx={{ width: '100%', mb: 2 }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
          <Tabs 
            value={tabValue} 
            onChange={handleTabChange}
            aria-label="treasure hunt tabs"
            centered
          >
            <Tab label="Input" {...a11yProps(0)} />
            <Tab label="Solution" {...a11yProps(1)} disabled={!solution} />
            <Tab label="History" {...a11yProps(2)} />
          </Tabs>
        </Box>

        {error && (
          <Alert severity="error" sx={{ m: 2 }}>
            {error}
          </Alert>
        )}

        {loading && (
          <Box sx={{ display: 'flex', justifyContent: 'center', p: 2 }}>
            <CircularProgress />
          </Box>
        )}

        <TabPanel value={tabValue} index={0}>
          <TreasureForm
            onSolve={handleSolvePuzzle}
            currentPuzzle={currentPuzzle}
          />
        </TabPanel>

        <TabPanel value={tabValue} index={1}>
          {solution && (
            <TreasureSolution 
              solution={solution}
              puzzle={currentPuzzle}
            />
          )}
        </TabPanel>

        <TabPanel value={tabValue} index={2}>
          <TreasureHistory 
            puzzles={savedPuzzles} 
            onLoad={handleLoadPuzzle}
          />
        </TabPanel>
      </Paper>
    </Container>
  );
}

export default TreasureHunt;
