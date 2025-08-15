using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWP.Repository.DTO
{
    public class PuzzleMapDTO
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Columnn { get; set; }
        public int MaxTarget { get; set; }
        public string Matrix { get; set; } = null!;
        public double? Result { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
