using BattleshipApi.Models.Enums;

namespace BattleshipApi.Models;

public class Cell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public CellType Type { get; set; }
}
