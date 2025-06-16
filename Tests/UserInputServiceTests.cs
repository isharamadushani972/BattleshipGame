using Microsoft.Extensions.Configuration;
using BattleshipApi.Models.Enums;
using BattleshipApi.Services;

namespace Tests;

[TestFixture]
public class UserInputServiceTests
{
    private GridService _gridService;
    private UserInputService _userInputService;

    [SetUp]
    public void Setup()
    {
        var configDict = new Dictionary<string, string> {
            {"GridSettings:GridSize", "10"}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();

        _gridService = new GridService(configuration);
        _userInputService = new UserInputService(_gridService, configuration);
    }

    [TestCase("A1", true)]
    [TestCase("J10", true)]
    [TestCase("A0", false)]
    [TestCase("K1", false)]
    [TestCase("A11", false)]
    [TestCase("1A", false)]
    [TestCase("", false)]
    [TestCase("   ", false)]
    [TestCase("Z5", false)]
    [TestCase("A", false)]
    [TestCase("A100", false)]
    public void IsValidInput_Should_Validate_Input_Correctly(string input, bool expectedIsValid)
    {
        var (isValid, _) = _userInputService.IsValidInput(input);

        Assert.AreEqual(expectedIsValid, isValid);
    }

    [Test]
    public void ProcessUserInput_Should_Return_Empty_Message_When_Cell_Is_Empty()
    {
        // Find an empty cell
        int row = -1, col = -1;

        for (int r = 0; r < 10; r++)
        {
            for (int c = 0; c < 10; c++)
            {
                var cell = _gridService.GetCell(r, c);
                if (cell.Type == CellType.Empty)
                {
                    row = r;
                    col = c;
                    break;
                }
            }
            if (row != -1) break;
        }

        Assert.AreNotEqual(-1, row, "No Empty cell found");

        string input = $"{(char)('A' + row)}{col + 1}";

        var result = _userInputService.ProcessUserInput(input);

        Assert.AreEqual("The cell is empty.", result);
    }

    [Test]
    public void ProcessUserInput_Should_Return_Already_Hit_Message()
    {
        _gridService.SetCell(0, 0, CellType.Hit);
        string input = "A1";

        var result = _userInputService.ProcessUserInput(input);

        Assert.AreEqual("It has already been hit.", result);
    }

    [Test]
    public void ProcessUserInput_Should_Return_Miss_Message()
    {
        _gridService.SetCell(0, 1, CellType.Miss);
        string input = "A2";

        var result = _userInputService.ProcessUserInput(input);

        Assert.AreEqual("The cell was shot but missed.", result);
    }
}
