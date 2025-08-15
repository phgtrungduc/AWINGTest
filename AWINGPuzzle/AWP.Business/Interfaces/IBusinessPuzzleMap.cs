using AWP.Business.Models;
using AWP.DBContext.Models;
using AWP.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWP.Business.Interfaces
{
    public interface IBusinessPuzzleMap : IBusinessBase<PuzzleMap, PuzzleMapDTO>
    {
        // Add any specific methods for PuzzleMap business layer here
        IEnumerable<PuzzleMapDTO> GetByRowAndColumn(int row, int column);
        IEnumerable<PuzzleMapDTO> GetByMaxTarget(int maxTarget);
        
        // Treasure hunt methods
        ServiceResult ValidateTreasureHunt(TreasureHuntInputDTO input);
        TreasureHuntResultDTO SolveTreasureHunt(TreasureHuntInputDTO input);
    }
}
