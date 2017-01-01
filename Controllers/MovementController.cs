using Microsoft.AspNetCore.Mvc;

using LanfeustBridge.Services;

namespace LanfeustBridge.Controllers
{
    [Route("api/[controller]")]
    public class MovementController : Controller
    {
        private readonly MovementService _movementService;

        public MovementController(MovementService movementService)
        {
            _movementService = movementService;
        }

        // GET: api/movement
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_movementService.GetAllMovements());
        }

        // GET api/movement/mitchell
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var movement = _movementService.GetMovementDescription(id);
            if (movement == null)
                return NotFound();
            return Ok(movement);
        }

        // GET api/movement/mitchell/validation?nbTables=2&nbRounds=2
        [HttpGet("{name}/validation")]
        public IActionResult GetValidation(string name, [FromQuery]int nbTables, [FromQuery]int nbRounds)
        {
            var validation = _movementService.Validate(name, nbTables, nbRounds);
            if (validation == null)
                return NotFound();
            return Ok(validation);
        }
    }
}
