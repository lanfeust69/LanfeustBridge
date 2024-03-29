namespace LanfeustBridge.Services;

public class DbTournamentsService : ITournamentService
{
    private readonly ILogger _logger;
    private readonly ILiteCollection<Tournament> _tournaments;
    private readonly IDealsService _dealsService;

    public DbTournamentsService(ILogger<DbTournamentsService> logger, IDealsService dealsService, DbService dbService)
    {
        _logger = logger;
        _dealsService = dealsService;
        _tournaments = dbService.Db.GetCollection<Tournament>();
        var toUpdate = _tournaments.FindAll().ToList();
        foreach (var tournament in toUpdate)
        {
            var movementId = tournament.Movement == "Individual" ? "individual12" : tournament.Movement.ToLower();
            if (movementId == tournament.Movement)
                continue;
            tournament.Movement = movementId;
            _tournaments.Update(tournament.Id, tournament);
            _logger.LogInformation("Tournament '{Name}' (Id {Id}) updated to {Movement}", tournament.Name, tournament.Id, movementId);
        }
    }

    public IEnumerable<(int, string)> GetNames()
    {
        return _tournaments.FindAll().Select(t => (t.Id, t.Name));
    }

    public Tournament GetTournament(int id)
    {
        return _tournaments.FindById(id);
    }

    public Tournament SaveTournament(Tournament tournament)
    {
        bool newTournament = tournament.Id == 0;
        _tournaments.Upsert(tournament);
        if (newTournament)
            _logger.LogInformation("New tournament created with Id {TournamentId}", tournament.Id);
        if (tournament.Status == TournamentStatus.Setup)
        {
            _dealsService.SetDealsForTournament(tournament.Id, tournament.CreateDeals());
            // define positions
            tournament.GeneratePositions();
            _tournaments.Update(tournament);
            _logger.LogInformation("Deals and positions created for tournament {TournamentId}", tournament.Id);
        }

        return tournament;
    }

    public bool DeleteTournament(int id)
    {
        return _tournaments.Delete(id);
    }
}
