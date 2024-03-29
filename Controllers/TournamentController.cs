namespace LanfeustBridge.Controllers;

[Route("api/[controller]")]
[Authorize]
public class TournamentController : Controller
{
    private readonly ILogger _logger;
    private readonly ITournamentService _tournamentService;
    private readonly IDealsService _dealsService;
    private readonly IHubContext<TournamentHub, ITournamentNotifier> _tournamentHubContext;

    public TournamentController(
        ILogger<TournamentController> logger,
        ITournamentService tournamentService,
        IDealsService dealsService,
        IHubContext<TournamentHub, ITournamentNotifier> tournamentHubContext)
    {
        _logger = logger;
        _tournamentService = tournamentService;
        _dealsService = dealsService;
        _tournamentHubContext = tournamentHubContext;
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
    [Authorize(Roles = "Admin")]
    public IActionResult Post([FromBody] Tournament tournament)
    {
        tournament = _tournamentService.SaveTournament(tournament);
        _tournamentHubContext.Clients.All.NewTournament();
        return Ok(tournament);
    }

    // PUT api/tournament/3
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Put(int id, [FromBody] Tournament tournament)
    {
        if (id != tournament.Id)
            return BadRequest($"Id mismatch : id of resource was {id}, id of tournament was {tournament.Id}");
        tournament = _tournamentService.SaveTournament(tournament);
        return Ok(tournament);
    }

    // DELETE api/tournament/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public bool Delete(int id)
    {
        return _tournamentService.DeleteTournament(id);
    }

    // GET api/tournament/scoring
    [HttpGet("scoring")]
    public IActionResult GetScorings()
    {
        return Ok(new[] { ScoringMethod.Matchpoint, ScoringMethod.IMP, ScoringMethod.Mixed, ScoringMethod.MixedTiedAt10 });
    }

    // POST api/tournament/3/start
    [HttpPost("{id}/start")]
    [Authorize(Roles = "Admin")]
    public IActionResult Start(int id)
    {
        var tournament = _tournamentService.GetTournament(id);
        if (tournament == null)
            return NotFound();
        if (tournament.Status != TournamentStatus.Setup)
            return BadRequest($"Cannot start tournament in status {tournament.Status}");

        tournament.Status = TournamentStatus.Running;
        tournament = _tournamentService.SaveTournament(tournament);
        _tournamentHubContext.Clients.All.TournamentStarted(id);
        return Ok(tournament);
    }

    // POST api/tournament/3/close
    [HttpPost("{id}/close")]
    [Authorize(Roles = "Admin")]
    public IActionResult Close(int id)
    {
        var tournament = _tournamentService.GetTournament(id);
        if (tournament == null)
            return NotFound();
        if (tournament.Status == TournamentStatus.Setup)
            return BadRequest($"Cannot close tournament in status {tournament.Status}");

        tournament.Close(_dealsService.GetDeals(id)!);
        tournament = _tournamentService.SaveTournament(tournament);
        _tournamentHubContext.Clients.All.TournamentFinished(id);
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

        bool finished = tournament.AreAllScoresEntered(_dealsService.GetDeals(id));

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
        _logger.LogInformation("Sending notification of round {CurrentRound} started", tournament.CurrentRound);
        _tournamentHubContext.Clients.All.NextRound(id, tournament.CurrentRound);
        return Ok();
    }
}
