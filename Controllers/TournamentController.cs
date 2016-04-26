using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

using LanfeustBridge.Models;
using LanfeustBridge.Services;

namespace LanfeustBridge.Controllers
{
    [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        private ITournamentService _service;

        public TournamentController(ITournamentService service)
        {
            _service = service;
        }

        // GET: api/tournament
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetNames().Select(n => new { Id = n.Item1, Name = n.Item2 }));
        }

        // GET api/tournament/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tournament = _service.GetTournament(id);
            if (tournament == null)
                return HttpNotFound();
            return Ok(tournament);
        }

        // POST api/tournament
        [HttpPost]
        public IActionResult Post(Tournament tournament)
        {
            tournament = _service.SaveTournament(tournament);
            return Ok(tournament);
        }

        // DELETE api/tournament/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _service.DeleteTournament(id);
        }
    }
}
