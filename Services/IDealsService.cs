namespace LanfeustBridge.Services
{
    using Models;

    public interface IDealsService
    {
        Deal GetDeal(int tournamentId, int dealId);

        Deal[] GetDeals(int tournamentId);

        Deal SaveDeal(int tournamentId, Deal deal);

        void SetDealsForTournament(int tournamentId, Deal[] deals);
    }
}
