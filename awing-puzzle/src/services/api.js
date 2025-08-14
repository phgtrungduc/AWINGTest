import axios from 'axios';

// Define base URL for API
const API_URL = 'http://localhost:5000/api'; // Change this to match your backend URL

// Create axios instance with base URL
const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// API service object
const apiService = {
  // Save treasure hunt scenario
  savePuzzle: async (puzzleData) => {
    try {
      const response = await apiClient.post('/treasurehunt/save', puzzleData);
      return response.data;
    } catch (error) {
      console.error('Error saving puzzle:', error);
      throw error;
    }
  },

  // Get saved puzzle by id
  getPuzzle: async (id) => {
    try {
      const response = await apiClient.get(`/treasurehunt/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error getting puzzle:', error);
      throw error;
    }
  },

  // Get all saved puzzles
  getAllPuzzles: async () => {
    try {
      const response = await apiClient.get('/treasurehunt');
      return response.data;
    } catch (error) {
      console.error('Error getting all puzzles:', error);
      throw error;
    }
  },

  // Solve puzzle and get minimum fuel required
  solvePuzzle: async (puzzleData) => {
    try {
      const response = await apiClient.post('/treasurehunt/solve', puzzleData);
      return response.data;
    } catch (error) {
      console.error('Error solving puzzle:', error);
      throw error;
    }
  },
};

export default apiService;
