using BattleshipApi.Models;
using BattleshipApi.Models.Enums;

namespace BattleshipApi.Services;

public class UserInputService
{
    private readonly GridService _gridService;

    public UserInputService(GridService gridService)
    {
        _gridService = gridService;
    }

    public string ProcessUserInput(string input)
    {
        int row = GetRowFromInput(input[0]);
        int column = GetColumnFromInput(int.Parse(input.Substring(1)));

        Cell aimedCell = _gridService.GetCell(row, column);

        switch (aimedCell.Type)
        {
            case CellType.Empty:
                _gridService.SetCell(row, column, CellType.Miss);
                return "The cell is empty.";

            case CellType.Battleship:
                _gridService.SetCell(row, column, CellType.Hit);
                return _gridService.WinnerStatus(CellType.Battleship);

            case CellType.Destroyership1:
                _gridService.SetCell(row, column, CellType.Hit);
                return _gridService.WinnerStatus(CellType.Destroyership1);

            case CellType.Destroyership2:
                _gridService.SetCell(row, column, CellType.Hit);
                return _gridService.WinnerStatus(CellType.Destroyership2);

            case CellType.Hit:
                return "It has already been hit.";

            case CellType.Miss:
                return "The cell was shot but missed.";

            default:
                return "Unknown cell type.";
        }
    }

    public (bool, string) IsValidInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return (false, "Invalid input. Please enter a valid command.");
        }
        input = input.Trim().ToUpper();

        if (input.Length < 2 || input.Length > 3)
        {
            return (false, "Invalid input length. Format must be like A5 or B10.");
        }

        char rowChar = input[0];
        string colChar = input.Substring(1);

        if (rowChar < 'A' || rowChar > 'J')
        {
            return (false, "Invalid row. It should be a letter from A to J.");
        }

        if (!int.TryParse(colChar, out int col) || col < 1 || col > 10)
        {
            return (false, "Invalid column. It should be a number from 1 to 10.");
        }

        return (true, "Valid input.");
    }

    private int GetRowFromInput(char row)
    {
        switch (row)
        {
            case 'A':
                return 0;
            case 'B':
                return 1;
            case 'C':
                return 2;
            case 'D':
                return 3;
            case 'E':
                return 4;
            case 'F':
                return 5;
            case 'G':
                return 6;
            case 'H':
                return 7;
            case 'I':
                return 8;
            case 'J':
                return 9;
            default: return 11;
        }
    }

    private int GetColumnFromInput(int column)
    {
        return column - 1;
    }
}
