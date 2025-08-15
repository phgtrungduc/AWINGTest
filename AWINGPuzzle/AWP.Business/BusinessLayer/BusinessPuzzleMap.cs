using AutoMapper;
using AWP.Business.Interfaces;
using AWP.Business.Models;
using AWP.DBContext.Models;
using AWP.Repository.DTO;
using AWP.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var result = new ServiceResult { StatusCode = 200 };

            if (input == null)
            {
				result.StatusCode = 400;
				result.Message = "Input data cannot be null";
                return result;
            }

            if (input.Matrix == null)
            {
				result.StatusCode = 400;
				result.Message = "Matrix cannot be null";
                return result;
            }

            if (input.Rows <= 0 || input.Rows > 500)
            {
                result.StatusCode = 400;
                result.Message = "Number of rows must be between 1 and 500";
                return result;
            }

            if (input.Columns <= 0 || input.Columns > 500)
            {
                result.StatusCode = 400;
                result.Message = "Number of columns must be between 1 and 500";
                return result;
            }

            if (input.MaxTarget <= 0 || input.MaxTarget > input.Rows * input.Columns)
            {
                result.StatusCode = 400;
                result.Message = $"Maximum target value must be between 1 and {input.Rows * input.Columns}";
                return result;
            }

            if (input.Matrix.GetLength(0) != input.Rows || input.Matrix.GetLength(1) != input.Columns)
            {
                result.StatusCode = 400;
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
                        result.StatusCode = 400;
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
                result.StatusCode = 400;
                result.Message = $"Matrix must contain exactly one cell with value {input.MaxTarget}";
                return result;
            }

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

            var chestPositions = new Dictionary<int, (int row, int col)>();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int chestNumber = matrix[i, j];
                    chestPositions[chestNumber] = (i, j);
                }
            }

            int currentKey = 0;
            double totalFuel = 0;
            var path = new List<TreasureHuntStep>();
            
            var currentPosition = (row: 0, col: 0);
            
            path.Add(new TreasureHuntStep
            {
                ChestNumber = 0,
                Row = 1, // Converting to 1-indexed for output
                Column = 1,
                FuelUsed = 0
            });

            while (currentKey < p)
            {
                int nextKey = currentKey + 1;
                
                // Find position of the next chest
                if (!chestPositions.TryGetValue(nextKey, out var nextPosition))
                {
                    throw new Exception($"Chest with number {nextKey} not found in the matrix.");
                }

                double fuel = CalculateFuel(currentPosition, nextPosition);
                totalFuel += fuel;

                currentPosition = nextPosition;
                currentKey = nextKey;

                path.Add(new TreasureHuntStep
                {
                    ChestNumber = nextKey,
                    Row = currentPosition.row + 1, 
                    Column = currentPosition.col + 1,
                    FuelUsed = fuel
                });
            }

            result.MinimumFuel = totalFuel;
            //result.Path = path;
            
            return result;
        }

        /// <summary>
        /// Calculates fuel needed to move between two positions
        /// </summary>
        private double CalculateFuel((int row, int col) from, (int row, int col) to)
        {
            // Using the formula: sqrt((x1-x2)^2 + (y1-y2)^2)
            double dx = from.row - to.row;
            double dy = from.col - to.col;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
