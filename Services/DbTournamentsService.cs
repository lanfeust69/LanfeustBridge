using System;
using System.Collections.Generic;
using System.Linq;

using LiteDB;

using Microsoft.Extensions.Logging;

using LanfeustBridge.Models;

namespace LanfeustBridge.Services
{
    public class DbTournamentsService : ITournamentService
    {
        private ILogger _logger;
        private LiteCollection<Tournament> _tournaments;
        private IDealsService _dealsService;

        public DbTournamentsService(ILogger<DbTournamentsService> logger, IDealsService dealsService, DbService dbService)
        {
            _logger = logger;
            _dealsService = dealsService;
            _tournaments = dbService.Db.GetCollection<Tournament>();
        }

        public IEnumerable<Tuple<int, string>> GetNames()
        {
            return _tournaments.FindAll().Select(t => Tuple.Create(t.Id, t.Name));
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
                _logger.LogInformation($"New tournament created with Id {tournament.Id}");
            if (tournament.Status == TournamentStatus.Setup)
            {
                _dealsService.SetDealsForTournament(tournament.Id, tournament.CreateDeals());
                // define positions
                tournament.GeneratePositions();
                _tournaments.Update(tournament);
                _logger.LogInformation($"Deals and positions created for tournament {tournament.Id}");
            }

            return tournament;
        }

        public bool DeleteTournament(int id)
        {
            return _tournaments.Delete(id);
        }
    }
}
