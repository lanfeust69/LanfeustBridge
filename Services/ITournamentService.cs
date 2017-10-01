using System;
using System.Collections.Generic;

using LanfeustBridge.Models;

namespace LanfeustBridge.Services
{
    public interface ITournamentService
    {
        IEnumerable<(int, string)> GetNames();
        Tournament GetTournament(int id);
        Tournament SaveTournament(Tournament tournament);
        bool DeleteTournament(int id);
    }
}
