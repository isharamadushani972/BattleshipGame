using BattleshipApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GridController : ControllerBase
    {
        private readonly GridService _gridService;
        private readonly UserInputService _userInputService;

        public GridController(GridService gridService, UserInputService userInputService)
        {
            _gridService = gridService;
            _userInputService = userInputService;
        }

        [HttpGet("actualgrid")]
        public IActionResult GetActualGrid()
        {
            string grid = _gridService.PrintActualGrid();
            return Ok(grid);
        }

        [HttpGet("usergrid")]
        public IActionResult GetUserGrid()
        {
            string grid = _gridService.PrintUserInputGrid();
            return Ok(grid);
        }

        [HttpPost]
        public IActionResult ProcessUserInput([FromBody] string value)
        {
            var (isValid, message) = _userInputService.IsValidInput(value);

            if (!isValid)
            {
                return BadRequest(message);
            }
            else
            {
                var result = _userInputService.ProcessUserInput(value);
                return Ok(result);
            }
        }

    }
}
