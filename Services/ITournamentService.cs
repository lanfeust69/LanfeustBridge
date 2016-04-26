using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LanfeustBridge.Models;

namespace LanfeustBridge.Services
{
    public interface ITournamentService
    {
        IEnumerable<Tuple<int, string>> GetNames();
        Tournament GetTournament(int id);
        Tournament SaveTournament(Tournament tournament);
        bool DeleteTournament(int id);
    }
}
