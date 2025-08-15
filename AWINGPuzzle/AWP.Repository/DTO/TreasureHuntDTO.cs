using System;
using System.Collections.Generic;

namespace AWP.Repository.DTO
{
    public class TreasureHuntInputDTO
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int MaxTarget { get; set; }
        public int[,] Matrix { get; set; }

	}

    public class TreasureHuntResultDTO
    {
        public double MinimumFuel { get; set; }
        //public List<TreasureHuntStep> Path { get; set; } = new List<TreasureHuntStep>();
        public Guid PuzzleId { get; set; }
    }

    public class TreasureHuntStep
    {
        public int ChestNumber { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public double FuelUsed { get; set; }
    }
}
