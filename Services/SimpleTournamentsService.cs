using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using LanfeustBridge.Models;
using System.IO;
using Newtonsoft.Json;

namespace LanfeustBridge.Services
{
    public class SimpleTournamentsService : ITournamentService
    {
        private int _nextId = 0;
        ILogger _logger;
        string _dataFile;

        private Lazy<Dictionary<int, Tournament>> _tournaments;

        private Dictionary<int, Tournament> Tournaments { get { return _tournaments.Value; } }

        public SimpleTournamentsService(ILogger<ITournamentService> logger, DirectoryService directoryService)
        {
            _logger = logger;
            _dataFile = Path.Combine(directoryService.DataDirectory, "tournaments.json");
            _tournaments = new Lazy<Dictionary<int, Tournament>>(InitializeTournaments);
        }

        private Dictionary<int, Tournament> InitializeTournaments()
        {
            Dictionary<int, Tournament> result;
            if (File.Exists(_dataFile))
                 result = JsonConvert.DeserializeObject<Dictionary<int, Tournament>>(File.ReadAllText(_dataFile));
            else
                result = new Dictionary<int, Tournament> { { 0, new Tournament { Id = 0, Name = "Test 1", Movement = "Mitchell" } } };
            if (result.Count > 0)
                _nextId = result.Keys.Max() + 1;
            _logger.LogInformation($"InitializeTournaments() done, {result.Count} tournaments in db, _nextId is {_nextId}");
            return result;
        }

        private int GetNextId()
        {
            return _nextId++;
        }

        public IEnumerable<Tuple<int, string>> GetNames()
        {
            return Tournaments.Values.Select(t => Tuple.Create(t.Id, t.Name));
        }

        public Tournament GetTournament(int id)
        {
            Tournament result;
            Tournaments.TryGetValue(id, out result);
            return result; // null if not found
        }

        public Tournament SaveTournament(Tournament tournament)
        {
            if (tournament.Id == -1)
            {
                tournament.Id = GetNextId();
                _logger.LogInformation($"New tournament created with Id {tournament.Id}");
            }
            Tournaments[tournament.Id] = tournament;
            File.WriteAllText(_dataFile, JsonConvert.SerializeObject(Tournaments));
            return tournament;
        }

        public bool DeleteTournament(int id)
        {
            bool removed = Tournaments.Remove(id);
            if (removed)
                File.WriteAllText(_dataFile, JsonConvert.SerializeObject(Tournaments));
            return removed;
        }
    }
}
