using AutoMapper;
using AWP.Business.Interfaces;
using AWP.Business.Models;
using AWP.DBContext.Models;
using AWP.Repository.DTO;
using AWP.Repository.Interfaces;

namespace AWP.Business.BusinessLayer
{
    public class BusinessPuzzleMap : BusinessBase<PuzzleMap, PuzzleMapDTO>, IBusinessPuzzleMap
    {
        private readonly IPuzzleMapRepository _puzzleMapRepository;

        public BusinessPuzzleMap(IPuzzleMapRepository puzzleMapRepository, IMapper mapper)
            : base(puzzleMapRepository, mapper)
        {
            _puzzleMapRepository = puzzleMapRepository;
        }

        public IEnumerable<PuzzleMapDTO> GetByRowAndColumn(int row, int column)
        {
            return _puzzleMapRepository.GetByRowAndColumn(row, column);
        }

        public IEnumerable<PuzzleMapDTO> GetByMaxTarget(int maxTarget)
        {
            return _puzzleMapRepository.GetByMaxTarget(maxTarget);
        }

        public override bool BeforeInsert(ref PuzzleMapDTO entity)
        {
            if (entity.CreatedAt == null)
            {
                entity.CreatedAt = DateTime.Now;
            }
            
            return base.BeforeInsert(ref entity);
        }

        public override bool ValidateCustom(PuzzleMapDTO entity)
        {
            if (entity.Row <= 0 || entity.Columnn <= 0)
            {
                _serviceResult.Message = "Row and Column must be greater than 0";
                return false;
            }

            if (entity.MaxTarget <= 0)
            {
                _serviceResult.Message = "MaxTarget must be greater than 0";
                return false;
            }

            if (string.IsNullOrEmpty(entity.Matrix))
            {
                _serviceResult.Message = "Matrix cannot be empty";
                return false;
            }

            return true;
        }

        public ServiceResult ValidateTreasureHunt(TreasureHuntInputDTO input)
        {
            var result = new ServiceResult { StatusCode = 400 };

            if (input == null)
            {
				result.Message = "Input data cannot be null";
                return result;
            }

            if (input.Matrix == null)
            {
				result.Message = "Matrix cannot be null";
                return result;
            }

            if (input.Rows <= 0 || input.Rows > 500)
            {
                result.Message = "Number of rows must be between 1 and 500";
                return result;
            }

            if (input.Columns <= 0 || input.Columns > 500)
            {
                result.Message = "Number of columns must be between 1 and 500";
                return result;
            }

            if (input.MaxTarget <= 0 || input.MaxTarget > input.Rows * input.Columns)
            {
                result.Message = $"Maximum target value must be between 1 and {input.Rows * input.Columns}";
                return result;
            }

            if (input.Matrix.GetLength(0) != input.Rows || input.Matrix.GetLength(1) != input.Columns)
            {
                result.Message = "Matrix dimensions do not match the provided rows and columns";
                return result;
            }

            bool foundMaxTarget = false;
            for (int i = 0; i < input.Rows; i++)
            {
                for (int j = 0; j < input.Columns; j++)
                {
                    if (input.Matrix[i, j] < 1 || input.Matrix[i, j] > input.MaxTarget)
                    {
                        result.Message = $"Matrix value at position [{i},{j}] is outside the valid range (1 to {input.MaxTarget})";
                        return result;
                    }

                    if (input.Matrix[i, j] == input.MaxTarget)
                    {
                        foundMaxTarget = true;
                    }
                }
            }

            if (!foundMaxTarget)
            {
                result.Message = $"Matrix must contain exactly one cell with value {input.MaxTarget}";
                return result;
            }

            result.StatusCode = 200;

            return result;
        }

        public TreasureHuntResultDTO SolveTreasureHunt(TreasureHuntInputDTO input)
        {
            var validationResult = ValidateTreasureHunt(input);
            if (validationResult.StatusCode != 200)
            {
                throw new ArgumentException(validationResult.Message);
            }
            
            var result = new TreasureHuntResultDTO();
            var n = input.Rows;
            var m = input.Columns;
            var p = input.MaxTarget;
            var matrix = input.Matrix;

            var chestPositions = new Dictionary<int, List<(int row, int col)>>();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int chestNumber = matrix[i, j];
                    if (!chestPositions.ContainsKey(chestNumber))
                    {
                        chestPositions[chestNumber] = new List<(int row, int col)>();
                    }
                    chestPositions[chestNumber].Add((i, j));
                }
            }

            var startPosition = (row: 0, col: 0);
            double minFuel = FindOptimalPath(chestPositions, p, startPosition);

            result.MinimumFuel = minFuel;
            
            return result;
        }

        private double CalculateFuel((int row, int col) from, (int row, int col) to)
        {
            double dx = from.row - to.row;
            double dy = from.col - to.col;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private double FindOptimalPath(
            Dictionary<int, List<(int row, int col)>> chestPositions, 
            int maxTarget,
            (int row, int col) startPosition)
        {
            var memo = new Dictionary<string, double>();

            var path = new Dictionary<int, (int row, int col)>();
            
            var res = OptimalPathRecursive(0, startPosition, chestPositions, maxTarget, memo, path);

            foreach (var step in path)
            {
                Console.WriteLine($"Chest {step.Key} at ({step.Value.row}, {step.Value.col})");
            }

            return res;
		}

        private double OptimalPathRecursive(
            int currentKey,
            (int row, int col) currentPosition,
            Dictionary<int, List<(int row, int col)>> chestPositions,
            int maxTarget,
            Dictionary<string, double> memo,
            Dictionary<int, (int row, int col)> path)
        {
            if (currentKey == maxTarget)
            {
                return 0;
            }

            string key = $"{currentKey}_{currentPosition.row}_{currentPosition.col}";
            if (memo.ContainsKey(key))
            {
                return memo[key];
            }

            int nextKey = currentKey + 1;
            if (!chestPositions.TryGetValue(nextKey, out var nextPositions) || !nextPositions.Any())
            {
                throw new Exception($"Chest with number {nextKey} not found in the matrix.");
            }

            double minTotalFuel = double.MaxValue;

            foreach (var nextPosition in nextPositions)
            {
                double fuelToNext = CalculateFuel(currentPosition, nextPosition);
                double remainingFuel = OptimalPathRecursive(nextKey, nextPosition, chestPositions, maxTarget, memo, path);
                
                double totalFuel = fuelToNext + remainingFuel;
                if (totalFuel < minTotalFuel)
                {
                    path[nextKey] = nextPosition;
                    minTotalFuel = totalFuel;
                }
            }

            memo[key] = minTotalFuel;
            return minTotalFuel;
        }
    }
}
