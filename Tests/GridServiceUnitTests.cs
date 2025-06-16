using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using BattleshipApi.Models.Enums;
using BattleshipApi.Services;

namespace Tests;

[TestFixture]
public class GridServiceTests
{
    private GridService _gridService;

    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"GridSettings:GridSize", "10"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _gridService = new GridService(configuration);
    }

    [Test]
    public void PrintActualGrid_Should_Display_Ships()
    {
        var output = _gridService.PrintActualGrid();

        Assert.IsTrue(output.Contains("B"), "Should contain Battleship B");
        Assert.IsTrue(output.Contains("D1"), "Should contain DestroyerShip1 D1");
        Assert.IsTrue(output.Contains("D2"), "Should contain DestroyerShip2 D2");
    }

    [Test]
    public void PrintUserInputGrid_Should_Hide_Ships_And_Show_Hits_Or_Misses()
    {
        _gridService.SetCell(0, 0, CellType.Hit);
        _gridService.SetCell(0, 1, CellType.Miss);

        var output = _gridService.PrintUserInputGrid();

        Assert.IsTrue(output.Contains("H"), "Should show Hit as H");
        Assert.IsTrue(output.Contains("M"), "Should show Miss as M");
        Assert.IsFalse(output.Contains("Ba"), "Should not show Battleship Ba");
        Assert.IsFalse(output.Contains("D1"), "Should not show DestroyerShip1 D1");
        Assert.IsFalse(output.Contains("D2"), "Should not show DestroyerShip2 D2");
    }

    [Test]
    public void GetCell_Should_Return_Correct_Cell()
    {
        var cell = _gridService.GetCell(2, 3);

        Assert.IsNotNull(cell);
        Assert.AreEqual(2, cell.Row);
        Assert.AreEqual(3, cell.Column);
    }

    [Test]
    public void SetCell_Should_Update_Cell_Type()
    {
        _gridService.SetCell(1, 1, CellType.Hit);

        var cell = _gridService.GetCell(1, 1);
        Assert.AreEqual(CellType.Hit, cell.Type);
    }

    [Test]
    public void WinnerStatus_Should_Return_Generic_Shot_Message_When_Not_Sunk()
    {
        // Hit any random cell that may or may not be a ship
        var cell = _gridService.GetCell(0, 0);
        _gridService.SetCell(cell.Row, cell.Column, CellType.Hit);

        var message = _gridService.WinnerStatus(cell.Type);

        Assert.AreEqual("yeee.. it was a shot!!", message);
    }
}
