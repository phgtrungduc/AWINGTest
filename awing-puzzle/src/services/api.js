import axios from 'axios';

// Define base URL for API
const API_URL = 'https://localhost:7009/api'; // Change this to match your backend URL

// Create axios instance with base URL
const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// API service object
const apiService = {

  // Get saved puzzle by id
  getPuzzle: async (id) => {
    try {
      const response = await apiClient.get(`/puzzlemap/${id}`);
      const puzzleMapDto = response.data;
      return {
        n: puzzleMapDto.Row,
        m: puzzleMapDto.Column,
        p: puzzleMapDto.MaxTarget,
        matrix: JSON.parse(puzzleMapDto.Matrix),
        id: puzzleMapDto.Id,
        result: puzzleMapDto.Result
      };
    } catch (error) {
      console.error('Error getting puzzle:', error);
      throw error;
    }
  },

  // Get all saved puzzles
  getAllPuzzles: async () => {
    try {
      const response = await apiClient.get('/puzzlemap/list');
      return response.data.map(puzzleMapDto => ({
        n: puzzleMapDto.Row,
        m: puzzleMapDto.Column,
        p: puzzleMapDto.MaxTarget,
        matrix: JSON.parse(puzzleMapDto.Matrix),
        id: puzzleMapDto.Id,
        createdAt: puzzleMapDto.CreatedAt,
        result: puzzleMapDto.Result
      }));
    } catch (error) {
      console.error('Error getting all puzzles:', error);
      throw error;
    }
  },

  // Solve puzzle and get minimum fuel required
  solvePuzzle: async (puzzleData) => {
    try {
      const input = {
        rows: puzzleData.n,
        columns: puzzleData.m,
        maxTarget: puzzleData.p,
        matrix: puzzleData.matrix
      };
      const response = await apiClient.post('/puzzlemap/solve', input);
      return response.data;
    } catch (error) {
      console.error('Error solving puzzle:', error);
      throw error;
    }
  },
};

export default apiService;
