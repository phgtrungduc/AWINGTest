using AutoMapper;
using AWP.Business.Interfaces;
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

        // Override base methods if needed
        public override bool BeforeInsert(ref PuzzleMapDTO entity)
        {
            // Set created date if it's null
            if (entity.CreatedAt == null)
            {
                entity.CreatedAt = DateTime.Now;
            }
            
            return base.BeforeInsert(ref entity);
        }

        public override bool ValidateCustom(PuzzleMapDTO entity)
        {
            // Add custom validation for PuzzleMap
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
    }
}
