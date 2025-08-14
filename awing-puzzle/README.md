# Treasure Hunt Puzzle Solver

This application solves the treasure hunt problem where pirates need to navigate through islands to find a treasure with minimal fuel consumption.

## Problem Statement

Pirates have found a treasure map of a sea area represented as an n × m matrix of islands. Each island contains a chest marked with a positive integer from 1 to p, and it contains the key for the chest marked with (x + 1). Only the chest marked with p (the highest number) contains the treasure.

The pirates start at island (1,1) with key 0. To travel from island (x1,y1) to island (x2,y2) requires fuel of √((x1-x2)² + (y1-y2)²).

The goal is to find the minimum fuel needed to retrieve the treasure.

## Features

- Input validation for matrix dimensions (n, m, p)
- Dynamic matrix input for island values
- Calculation of minimum fuel required
- Visualization of the optimal path
- History of saved puzzles

## Tech Stack

- **Frontend**: React with Material-UI
- **Backend**: C# ASP.NET Core (implemented separately)
- **Communication**: Axios for API requests

## Getting Started

### Prerequisites

- Node.js
- npm

### Installation

1. Clone the repository
   ```
   git clone https://github.com/yourusername/treasure-hunt.git
   cd treasure-hunt
   ```

2. Install dependencies
   ```
   npm install
   ```

3. Start the development server
   ```
   npm start
   ```

4. Open [http://localhost:3000](http://localhost:3000) to view the application in your browser

## Usage

1. Enter the dimensions:
   - n: Number of rows (1-500)
   - m: Number of columns (1-500)
   - p: Highest chest number (1 to n*m)

2. Fill in the matrix values:
   - Each cell represents an island with a chest
   - Values must be between 1 and p
   - Exactly one island must have value p

3. Click "Solve Puzzle" to calculate the minimum fuel

4. View the solution and path taken

5. Access previously saved puzzles in the History tab

## API Endpoints

The frontend expects the following API endpoints:

- `POST /api/treasurehunt/save`: Save a puzzle scenario
- `GET /api/treasurehunt`: Get all saved puzzles
- `GET /api/treasurehunt/{id}`: Get a specific puzzle
- `POST /api/treasurehunt/solve`: Solve a puzzle

## Backend Implementation

The backend should implement the treasure hunt algorithm using BFS or Dijkstra's algorithm to find the optimal path. The solution should return:

- Minimum fuel required
- Path taken through the islands
- Keys found along the way

## License

[MIT](https://opensource.org/licenses/MIT)