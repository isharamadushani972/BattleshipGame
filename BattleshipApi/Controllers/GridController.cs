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

        public GridController(GridService gridService)
        {
            _gridService = gridService;
        }

        [HttpGet]
        public IActionResult GetGrid()
        {
            return Ok(_gridService.PrintGrid());
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
                var result=_userInputService.ProcessUserInput(value);
                return Ok(result);
            }
        }

    }
}
