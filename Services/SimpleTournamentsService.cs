using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace LanfeustBridge.Services
{
    using LanfeustBridge.Models;

    public class SimpleTournamentsService : ITournamentService
    {
        private int _nextId = 1;
        private readonly ILogger _logger;
        private readonly IDealsService _dealsService;
        private readonly string _dataFile;

        private readonly Lazy<Dictionary<int, Tournament>> _tournaments;

        public SimpleTournamentsService(
            ILogger<ITournamentService> logger,
            DirectoryService directoryService,
            IDealsService dealsService)
        {
            _logger = logger;
            _dealsService = dealsService;
            _dataFile = Path.Combine(directoryService.DataDirectory, "tournaments.json");
            _tournaments = new Lazy<Dictionary<int, Tournament>>(InitializeTournaments);
        }

        private Dictionary<int, Tournament> Tournaments => _tournaments.Value;

        public IEnumerable<(int, string)> GetNames()
        {
            return Tournaments.Values.Select(t => (t.Id, t.Name));
        }

        public Tournament GetTournament(int id)
        {
            Tournaments.TryGetValue(id, out var result);
            return result; // null if not found
        }

        public Tournament SaveTournament(Tournament tournament)
        {
            if (tournament.Id == 0)
            {
                tournament.Id = GetNextId();
                _logger.LogInformation($"New tournament created with Id {tournament.Id}");
            }
            if (tournament.Status == TournamentStatus.Setup)
            {
                _dealsService.SetDealsForTournament(tournament.Id, tournament.CreateDeals());
                // define positions
                tournament.GeneratePositions();
            }
            Tournaments[tournament.Id] = tournament;
            SaveToFile();
            return tournament;
        }

        public bool DeleteTournament(int id)
        {
            bool removed = Tournaments.Remove(id);
            if (removed)
                SaveToFile();
            return removed;
        }

        private Dictionary<int, Tournament> InitializeTournaments()
        {
            Dictionary<int, Tournament> result;
            if (File.Exists(_dataFile))
                result = JsonConvert.DeserializeObject<Dictionary<int, Tournament>>(File.ReadAllText(_dataFile));
            else
                result = new Dictionary<int, Tournament>();

            if (result.Count > 0)
                _nextId = result.Keys.Max() + 1;
            _logger.LogInformation($"InitializeTournaments() done, {result.Count} tournaments in db, _nextId is {_nextId}");
            return result;
        }

        private int GetNextId()
        {
            return _nextId++;
        }

        private void SaveToFile()
        {
            File.WriteAllText(_dataFile, JsonConvert.SerializeObject(Tournaments));
        }
    }
}
