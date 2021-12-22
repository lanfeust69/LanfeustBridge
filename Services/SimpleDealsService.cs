using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace LanfeustBridge.Services
{
    using LanfeustBridge.Models;

    public class SimpleDealsService : IDealsService
    {
        private readonly ILogger _logger;
        private readonly string _dataFile;

        private readonly Lazy<Dictionary<int, Deal[]>> _deals;

        public SimpleDealsService(ILogger<IDealsService> logger, DirectoryService directoryService)
        {
            _logger = logger;
            _dataFile = Path.Combine(directoryService.DataDirectory, "deals.json");
            _deals = new Lazy<Dictionary<int, Deal[]>>(InitializeDeals);
        }

        private Dictionary<int, Deal[]> Deals => _deals.Value;

        public Deal? GetDeal(int tournamentId, int dealId)
        {
            Deals.TryGetValue(tournamentId, out var deals);
            if (deals == null || dealId > deals.Length)
                return null;
            return deals[dealId - 1];
        }

        public Deal[]? GetDeals(int tournamentId)
        {
            Deals.TryGetValue(tournamentId, out var deals);
            return deals;
        }

        public Deal? SaveDeal(int tournamentId, Deal deal)
        {
            Deals.TryGetValue(tournamentId, out var deals);
            if (deals == null || deal.Id > deals.Length)
                return null;
            deals[deal.Id - 1] = deal;
            SaveToFile();
            _logger.LogInformation("Deal {Deal} of tournament {Tournament} saved", deal.Id, tournamentId);
            return deal;
        }

        public void SetDealsForTournament(int tournamentId, Deal[] deals)
        {
            Deals[tournamentId] = deals;
            SaveToFile();
            _logger.LogInformation("Deals for tournament {Tournament} created", tournamentId);
        }

        private Dictionary<int, Deal[]> InitializeDeals()
        {
            Dictionary<int, Deal[]> result;
            if (File.Exists(_dataFile))
                result = JsonConvert.DeserializeObject<Dictionary<int, Deal[]>>(File.ReadAllText(_dataFile));
            else
                result = new Dictionary<int, Deal[]>();

            _logger.LogInformation("InitializeDeals() done, deals from {Count} tournaments in db", result.Count);
            return result;
        }

        private void SaveToFile()
        {
            File.WriteAllText(_dataFile, JsonConvert.SerializeObject(Deals));
        }
    }
}
