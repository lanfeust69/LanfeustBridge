using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

using LanfeustBridge.Models;
using LanfeustBridge.Services;

namespace LanfeustBridge.Controllers
{
    [Route("api/tournament/{tournamentId}/deal")]
    public class DealController : Controller
    {
        ILogger _logger;
        private IDealsService _dealsService;
        private ITournamentService _tournamentService;
        

        public DealController(ILogger<DealController> logger, IDealsService dealsService, ITournamentService tournamentService)
        {
            _logger = logger;
            _dealsService = dealsService;
            _tournamentService = tournamentService;
        }

        // GET api/tournament/1/deal/3
        [HttpGet("{id}")]
        public IActionResult Get(int tournamentId, int id)
        {
            var deal = _dealsService.GetDeal(tournamentId, id);
            if (deal == null)
                return HttpNotFound();
            return Ok(deal);
        }

        // GET api/tournament/1/deal
        [HttpGet()]
        public IActionResult Get(int tournamentId)
        {
            var deals = _dealsService.GetDeals(tournamentId);
            if (deals == null)
                return HttpNotFound();
            return Ok(deals);
        }

        // GET api/tournament/1/deal/3/score/2
        [HttpGet("{id}/score/{round}")]
        public IActionResult GetScore(int tournamentId, int id, int round)
        {
            var deal = _dealsService.GetDeal(tournamentId, id);
            if (deal == null || round < 0 || round >= deal.Scores.Length)
                return HttpNotFound();
            return Ok(deal.Scores[round]);
        }

        // POST api/tournament/1/deal/3/score/2
        [HttpPost("{id}/score/{round}")]
        public IActionResult PostScore(int tournamentId, int id, int round, [FromBody]Score score)
        {
            _logger.LogInformation($"Receiving score for deal {id}, round {round} : {score.Entered}, {score.Tricks}, {score.BridgeScore}");
            var deal = _dealsService.GetDeal(tournamentId, id);
            if (deal == null || round < 0 || round >= deal.Scores.Length)
                return HttpNotFound();
            deal.Scores[round] = score;
            deal.ComputeResults(_tournamentService.GetTournament(tournamentId).Scoring);
            _dealsService.SaveDeal(tournamentId, deal);
            return Ok(score);
        }
    }
}
