using AutoMapper;
using AWP.DBContext.Models;
using AWP.Repository.DTO;
using AWP.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWP.Repository.Implements
{
    public class PuzzleMapRepository : RepositoryBase<PuzzleMap, PuzzleMapDTO>, IPuzzleMapRepository
    {
        public PuzzleMapRepository(AwingDbContext context, IMapper mapper) : base(context, mapper)
        {
            _dbSet = _context.PuzzleMaps;
        }

        public IEnumerable<PuzzleMapDTO> GetByRowAndColumn(int row, int column)
        {
            var puzzleMaps = _dbSet.Where(p => p.Row == row && p.Columnn == column).ToList();
            return _mapper.Map<List<PuzzleMap>, IEnumerable<PuzzleMapDTO>>(puzzleMaps);
        }

        public IEnumerable<PuzzleMapDTO> GetByMaxTarget(int maxTarget)
        {
            var puzzleMaps = _dbSet.Where(p => p.MaxTarget == maxTarget).ToList();
            return _mapper.Map<List<PuzzleMap>, IEnumerable<PuzzleMapDTO>>(puzzleMaps);
        }

        // Override base methods if needed
        public override IEnumerable<PuzzleMapDTO> GetAll()
        {
            var puzzleMaps = _dbSet.OrderByDescending(p => p.CreatedAt).ToList();
            return _mapper.Map<List<PuzzleMap>, IEnumerable<PuzzleMapDTO>>(puzzleMaps);
        }

        public override PuzzleMap GetByID(string id)
        {
            return _dbSet.Find(Guid.Parse(id));
        }
    }
}
