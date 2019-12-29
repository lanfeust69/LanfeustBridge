using System.Linq;

using LiteDB;

using Microsoft.Extensions.Logging;

namespace LanfeustBridge.Services
{
    using LanfeustBridge.Models;

    public class DbDealsService : IDealsService
    {
        private readonly ILogger _logger;
        private readonly LiteCollection<DealWrapper> _deals;

        public DbDealsService(ILogger<DbDealsService> logger, DbService dbService)
        {
            _logger = logger;
            _deals = dbService.Db.GetCollection<DealWrapper>();
            _deals.EnsureIndex(d => d.TournamentId);
        }

        public Deal GetDeal(int tournamentId, int dealId)
        {
            return _deals.FindById($"{tournamentId}/{dealId}")?.Deal;
        }

        public Deal[] GetDeals(int tournamentId)
        {
            return _deals.Find(d => d.TournamentId == tournamentId).Select(d => d.Deal).OrderBy(d => d.Id).ToArray();
        }

        public Deal SaveDeal(int tournamentId, Deal deal)
        {
            _deals.Upsert(new DealWrapper(tournamentId, deal));
            return deal;
        }

        public void SetDealsForTournament(int tournamentId, Deal[] deals)
        {
            foreach (var deal in deals)
                _deals.Upsert(new DealWrapper(tournamentId, deal));
            _logger.LogInformation($"Deals for tournament {tournamentId} saved");
        }

        public class DealWrapper
        {
            public DealWrapper()
            {
            }

            public DealWrapper(int tournamementId, Deal deal)
            {
                TournamentId = tournamementId;
                Deal = deal;
            }

            public int TournamentId { get; set; }

            public Deal Deal { get; set; }

            public string Id => $"{TournamentId}/{Deal.Id}";
        }
    }
}
