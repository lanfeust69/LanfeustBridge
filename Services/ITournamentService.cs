using System;
using System.Collections.Generic;

namespace LanfeustBridge.Services
{
    using LanfeustBridge.Models;

    public interface ITournamentService
    {
        IEnumerable<(int, string)> GetNames();

        Tournament? GetTournament(int id);

        Tournament SaveTournament(Tournament tournament);

        bool DeleteTournament(int id);
    }
}
