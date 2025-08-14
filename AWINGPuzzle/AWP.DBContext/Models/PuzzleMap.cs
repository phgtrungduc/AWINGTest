using System;
using System.Collections.Generic;

namespace AWP.DBContext.Models;

public partial class PuzzleMap
{
    public Guid Id { get; set; }

    public int Row { get; set; }

    public int Columnn { get; set; }

    public int MaxTarget { get; set; }

    public string Matrix { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
