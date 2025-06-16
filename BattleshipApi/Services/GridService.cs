using BattleshipApi.Models;
using BattleshipApi.Models.Enums;

namespace BattleshipApi.Services;

public class GridService
{
    private readonly int _gridSize;
    private readonly Cell[,] _grid;
    private readonly List<Cell> Battleship;
    private readonly List<Cell> DestroyerShip1;
    private readonly List<Cell> DestroyerShip2;

    public GridService(IConfiguration configuration)
    {
        _gridSize = configuration.GetValue<int>("GridSettings:GridSize");
        _grid = new Cell[_gridSize, _gridSize];
        InitializeGrid();
        Battleship = PlaceBattleship();
        DestroyerShip1 = Destroyership1();
        DestroyerShip2 = Destroyership2();
    }

    private void InitializeGrid()
    {
        for (int row = 0; row < _gridSize; row++)
        {
            for (int col = 0; col < _gridSize; col++)
            {
                _grid[row, col] = new Cell
                {
                    Row = row,
                    Column = col,
                    Type = CellType.Empty
                };
            }
        }
    }

    private List<Cell> PlaceBattleship()
    {
        return PlaceShip((int)ShipSize.Battleship, CellType.Battleship);
    }

    private List<Cell> Destroyership1()
    {
        return PlaceShip((int)ShipSize.DestroyerShip, CellType.Destroyership1);
    }

    private List<Cell> Destroyership2()
    {
        return PlaceShip((int)ShipSize.DestroyerShip, CellType.Destroyership2);
    }

    private List<Cell> PlaceShip(int shipSize, CellType type)
    {
        bool placed = false;
        List<Cell> shipCells = new List<Cell>();

        Random _random = new Random();

        while (!placed)
        {
            shipCells.Clear();

            bool isHorizontal = _random.Next(2) == 0;

            int startRow = _random.Next(0, isHorizontal ? _gridSize : _gridSize - shipSize + 1);
            int startCol = _random.Next(0, isHorizontal ? _gridSize - shipSize + 1 : _gridSize);

            bool overlaps = false;

            for (int i = 0; i < shipSize; i++)
            {
                int row = isHorizontal ? startRow : startRow + i;
                int col = isHorizontal ? startCol + i : startCol;

                if (_grid[row, col].Type != CellType.Empty)
                {
                    overlaps = true;
                    break;
                }

                shipCells.Add(_grid[row, col]);
            }

            if (!overlaps)
            {
                foreach (var cell in shipCells)
                {
                    _grid[cell.Row, cell.Column].Type = type;
                }
                placed = true;
            }
        }

        return shipCells;
    }

    public string PrintActualGrid()
    {
        var sb = new System.Text.StringBuilder();

        sb.Append("   ");
        for (int col = 1; col <= _gridSize; col++)
        {
            sb.Append($"{col,2} ");
        }
        sb.AppendLine();

        for (int row = 0; row < _gridSize; row++)
        {
            char rowLabel = (char)('A' + row);
            sb.Append($"{rowLabel}  ");

            for (int col = 0; col < _gridSize; col++)
            {
                var cell = _grid[row, col];
                string displayChar = cell.Type switch
                {
                    CellType.Battleship => "Ba",
                    CellType.Destroyership1 => "D1",
                    CellType.Destroyership2 => "D2",
                    CellType.Hit => "H",
                    CellType.Miss => "M",
                    _ => "."
                };

                sb.Append($"{displayChar,2} ");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public string PrintUserInputGrid()
    {
        var sb = new System.Text.StringBuilder();

        sb.Append("   ");
        for (int col = 1; col <= _gridSize; col++)
        {
            sb.Append($"{col,2} ");
        }
        sb.AppendLine();

        for (int row = 0; row < _gridSize; row++)
        {
            char rowLabel = (char)('A' + row);
            sb.Append($"{rowLabel}  ");

            for (int col = 0; col < _gridSize; col++)
            {
                var cell = _grid[row, col];
                string displayChar = cell.Type switch
                {
                    CellType.Hit => "H",
                    CellType.Miss => "M",
                    _ => "."
                };

                sb.Append($"{displayChar,2} ");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }


    public Cell GetCell(int row, int col) => _grid[row, col];

    public void SetCell(int row, int col, CellType type) => _grid[row, col].Type = type;

    public string WinnerStatus(CellType cellType)
    {
        bool isBattleshipSunk = Battleship.All(cell => cell.Type == CellType.Hit);
        bool isDestroyerShip1Sunk = DestroyerShip1.All(cell => cell.Type == CellType.Hit);
        bool isDestroyerShip2Sunk = DestroyerShip2.All(cell => cell.Type == CellType.Hit);

        if (isBattleshipSunk && isDestroyerShip1Sunk && isDestroyerShip2Sunk)
        {
            return ("woooow.... you are the winner!!");
        }
        else if (cellType == CellType.Battleship && isBattleshipSunk)
        {
            return "Battleship sunk..!";
        }
        else if (cellType == CellType.Destroyership1 && isDestroyerShip1Sunk)
        {
            return "Destroyership1 sunk..!";
        }
        else if (cellType == CellType.Destroyership2 && isDestroyerShip2Sunk)
        {
            return "Destroyership2 sunk..!";
        }
        else
        {
            return "yeee.. it was a shot!!";
        }
    }
}
