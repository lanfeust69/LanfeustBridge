namespace LanfeustBridge.Controllers;

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
        var tournament = _tournamentService.GetTournament(tournamentId);
        if (tournament == null || id > tournament.NbDeals)
            return NotFound();
        var deal = _dealsService.GetDeal(tournamentId, id);
        if (deal == null)
            return NotFound();
        return Ok(new { Deal = deal, HasNext = id < tournament.NbDeals });
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

    // GET api/tournament/1/deal/3/score/2/1
    [HttpGet("{id}/score/{round}/{table}")]
    public IActionResult GetScore(int tournamentId, int id, int round, int table)
    {
        var deal = _dealsService.GetDeal(tournamentId, id);
        if (deal == null)
            return NotFound();
        var score = deal.Scores.FirstOrDefault(s => s.Round == round && s.Table == table);
        if (score == null)
            return NotFound();
        return Ok(score);
    }

    // POST api/tournament/1/deal/3/score
    [HttpPost("{id}/score")]
    public IActionResult PostScore(int tournamentId, int id, [FromBody] Score score)
    {
        _logger.LogInformation("Receiving score for deal {Deal}, round {Round}, table {Table} : {EnteredScored}, {Tricks}, {BridgeScore}",
            id, score.Round, score.Table, score.Entered, score.Tricks, score.BridgeScore);
        var deal = _dealsService.GetDeal(tournamentId, id);
        if (deal == null)
            return NotFound();
        int idx = 0;
        while (idx < deal.Scores.Length && (deal.Scores[idx].Round != score.Round || deal.Scores[idx].Table != score.Table))
            idx++;
        if (idx == deal.Scores.Length)
            return NotFound();
        deal.Scores[idx] = score;
        var tournament = _tournamentService.GetTournament(tournamentId) ?? throw new InvalidOperationException($"Tournament {tournamentId} not found");
        deal.ComputeResults(tournament.Scoring);
        _dealsService.SaveDeal(tournamentId, deal);
        if (tournament.AreAllScoresEntered(_dealsService.GetDeals(tournamentId)))
        {
            _logger.LogInformation("Sending notification of round {Round} finished", score.Round);
            _tournamentHubContext.Clients.All.RoundFinished(tournamentId, score.Round);
        }

        return Ok(score);
    }
}
