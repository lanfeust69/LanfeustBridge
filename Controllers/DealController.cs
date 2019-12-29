using System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace LanfeustBridge.Controllers
{
    using LanfeustBridge.Hubs;
    using LanfeustBridge.Models;
    using LanfeustBridge.Services;

    [Route("api/tournament/{tournamentId}/deal")]
    [Authorize]
    public class DealController : Controller
    {
        private readonly ILogger _logger;
        private readonly IDealsService _dealsService;
        private readonly ITournamentService _tournamentService;
        private readonly IHubContext<TournamentHub, ITournamentNotifier> _tournamentHubContext;

        public DealController(
            ILogger<DealController> logger,
            IDealsService dealsService,
            ITournamentService tournamentService,
            IHubContext<TournamentHub, ITournamentNotifier> tournamentHubContext)
        {
            _logger = logger;
            _dealsService = dealsService;
            _tournamentService = tournamentService;
            _tournamentHubContext = tournamentHubContext;
        }

        // GET api/tournament/1/deal/3
        [HttpGet("{id}")]
        public IActionResult Get(int tournamentId, int id)
        {
            var deal = _dealsService.GetDeal(tournamentId, id);
            if (deal == null)
                return NotFound();
            return Ok(deal);
        }

        // GET api/tournament/1/deal
        [HttpGet]
        public IActionResult Get(int tournamentId)
        {
            var deals = _dealsService.GetDeals(tournamentId);
            if (deals == null)
                return NotFound();
            return Ok(deals);
        }

        // GET api/tournament/1/deal/3/score/2
        [HttpGet("{id}/score/{round}")]
        public IActionResult GetScore(int tournamentId, int id, int round)
        {
            var deal = _dealsService.GetDeal(tournamentId, id);
            if (deal == null || round < 0 || round >= deal.Scores.Length)
                return NotFound();
            return Ok(deal.Scores[round]);
        }

        // POST api/tournament/1/deal/3/score/2
        [HttpPost("{id}/score/{round}")]
        public IActionResult PostScore(int tournamentId, int id, int round, [FromBody]Score score)
        {
            _logger.LogInformation($"Receiving score for deal {id}, round {round} : {score.Entered}, {score.Tricks}, {score.BridgeScore}");
            var deal = _dealsService.GetDeal(tournamentId, id);
            if (deal == null || round < 0 || round >= deal.Scores.Length)
                return NotFound();
            deal.Scores[round] = score;
            var tournament = _tournamentService.GetTournament(tournamentId) ?? throw new Exception($"Tournament {tournamentId} not found");
            deal.ComputeResults(tournament.Scoring);
            _dealsService.SaveDeal(tournamentId, deal);
            if (tournament.AreAllScoresEntered(_dealsService.GetDeals(tournamentId)))
            {
                _logger.LogInformation($"Sending notification of round {round} finished");
                _tournamentHubContext.Clients.All.RoundFinished(tournamentId, round);
            }

            return Ok(score);
        }
    }
}
