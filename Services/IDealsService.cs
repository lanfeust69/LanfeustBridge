using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LanfeustBridge.Models;

namespace LanfeustBridge.Services
{
    public interface IDealsService
    {
        Deal GetDeal(int tournamentId, int dealId);
        Deal SaveDeal(int tournamentId, Deal deal);
        void SetDealsForTournament(int tournamentId, Deal[] deals);
    }
}
