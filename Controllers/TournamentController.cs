using System.Linq;

using Microsoft.AspNetCore.Mvc;

using LanfeustBridge.Models;
using LanfeustBridge.Services;

namespace LanfeustBridge.Controllers
{
    [Route("api/[controller]")]
    public class TournamentController : Controller
    {
        private readonly ITournamentService _tournamentService;
        private readonly IDealsService _dealsService;

        public TournamentController(ITournamentService tournamentService, IDealsService dealsService)
        {
            _tournamentService = tournamentService;
            _dealsService = dealsService;
        }

        // GET: api/tournament
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_tournamentService.GetNames().Select(n => new { Id = n.Item1, Name = n.Item2 }));
        }

        // GET api/tournament/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tournament = _tournamentService.GetTournament(id);
            if (tournament == null)
                return NotFound();
            return Ok(tournament);
        }

        // POST api/tournament
        [HttpPost]
        public IActionResult Post([FromBody]Tournament tournament)
        {
            tournament = _tournamentService.SaveTournament(tournament);
            return Ok(tournament);
        }

        // PUT api/tournament/3
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Tournament tournament)
        {
            if (id != tournament.Id)
                return BadRequest($"Id mismatch : id of resource was {id}, id of tournament was {tournament.Id}");
            tournament = _tournamentService.SaveTournament(tournament);
            return Ok(tournament);
        }

        // DELETE api/tournament/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _tournamentService.DeleteTournament(id);
        }

        // GET api/tournament/movement
        [HttpGet("movement")]
        public IActionResult GetMovements()
        {
            return Ok(new[]
                {
                    new { Name = "Mitchell", NbTables = -1 },
                    new { Name = "Teams", NbTables = 2 },
                    new { Name = "Triplicate", NbTables = 3 },
                    new { Name = "Individual", NbTables = 3 }
                });
        }

        // GET api/tournament/scoring
        [HttpGet("scoring")]
        public IActionResult GetScorings()
        {
            return Ok(new[] { "Matchpoint", "IMP" });
        }

        // POST api/tournament/3/start
        [HttpPost("{id}/start")]
        public IActionResult Start(int id)
        {
            var tournament = _tournamentService.GetTournament(id);
            if (tournament == null)
                return NotFound();
            if (tournament.Status != TournamentStatus.Setup)
                return BadRequest($"Cannot start tournament in status {tournament.Status}");

            tournament.Status = TournamentStatus.Running;
            tournament = _tournamentService.SaveTournament(tournament);
            return Ok(tournament);
        }

        // POST api/tournament/3/close
        [HttpPost("{id}/close")]
        public IActionResult Close(int id)
        {
            var tournament = _tournamentService.GetTournament(id);
            if (tournament == null)
                return NotFound();
            if (tournament.Status == TournamentStatus.Setup)
                return BadRequest($"Cannot close tournament in status {tournament.Status}");

            tournament.Close(_dealsService.GetDeals(id));
            tournament = _tournamentService.SaveTournament(tournament);
            return Ok(tournament);
        }

        // GET api/tournament/3/current-round
        [HttpGet("{id}/current-round")]
        public IActionResult GetCurrentRound(int id)
        {
            var tournament = _tournamentService.GetTournament(id);
            if (tournament == null)
                return NotFound();
            if (tournament.Status != TournamentStatus.Running)
                return BadRequest($"Cannot get current round of tournament in status {tournament.Status}");

            bool finished = tournament.Positions[tournament.CurrentRound]
                .SelectMany(p => p.Deals)
                .Distinct()
                .All(d => _dealsService.GetDeal(id, d).Scores[tournament.CurrentRound].Entered);

            return Ok(new { Round = tournament.CurrentRound, Finished = finished });
        }

        // POST api/tournament/3/next-round
        [HttpPost("{id}/next-round")]
        public IActionResult NextRound(int id)
        {
            var tournament = _tournamentService.GetTournament(id);
            if (tournament == null)
                return NotFound();
            if (tournament.Status != TournamentStatus.Running)
                return BadRequest($"Cannot get current round of tournament in status {tournament.Status}");

            tournament.CurrentRound++;
            _tournamentService.SaveTournament(tournament);
            return Ok();
        }
    }
}
