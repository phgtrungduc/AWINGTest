using AWP.DBContext.Models;
using AWP.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWP.Repository.Interfaces
{
    public interface IPuzzleMapRepository : IRepositoryBase<PuzzleMap, PuzzleMapDTO>
    {
        // Add any specific methods for PuzzleMap repository here
        IEnumerable<PuzzleMapDTO> GetByRowAndColumn(int row, int column);
        IEnumerable<PuzzleMapDTO> GetByMaxTarget(int maxTarget);
    }
}
