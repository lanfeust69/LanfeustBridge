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
    public class SimpleDealsService : IDealsService
    {
        ILogger _logger;
        string _dataFile;

        private Lazy<Dictionary<int, Deal[]>> _deals;

        private Dictionary<int, Deal[]> Deals { get { return _deals.Value; } }

        public SimpleDealsService(ILogger<IDealsService> logger, DirectoryService directoryService)
        {
            _logger = logger;
            _dataFile = Path.Combine(directoryService.DataDirectory, "deals.json");
            _deals = new Lazy<Dictionary<int, Deal[]>>(InitializeDeals);
        }

        private Dictionary<int, Deal[]> InitializeDeals()
        {
            Dictionary<int, Deal[]> result;
            if (File.Exists(_dataFile))
                result = JsonConvert.DeserializeObject<Dictionary<int, Deal[]>>(File.ReadAllText(_dataFile));
            else
                result = new Dictionary<int, Deal[]>();

            _logger.LogInformation($"InitializeDeals() done, deals from {result.Count} tournaments in db");
            return result;
        }

        public Deal GetDeal(int tournamentId, int dealId)
        {
            Deal[] deals;
            Deals.TryGetValue(tournamentId, out deals);
            if (deals == null || dealId > deals.Length)
                return null;
            return deals[dealId - 1];
        }

        public Deal SaveDeal(int tournamentId, Deal deal)
        {
            Deal[] deals;
            Deals.TryGetValue(tournamentId, out deals);
            if (deals == null || deal.Id > deals.Length)
                return null;
            deals[deal.Id - 1] = deal;
            SaveToFile();
            _logger.LogInformation($"Deal {deal.Id} of tournament {tournamentId} saved");
            return deal;
        }

        public void SetDealsForTournament(int tournamentId, Deal[] deals)
        {
            Deals[tournamentId] = deals;
            SaveToFile();
            _logger.LogInformation($"Deals for tournament {tournamentId} created");
        }

        private void SaveToFile()
        {
            File.WriteAllText(_dataFile, JsonConvert.SerializeObject(Deals));
        }
    }
}
