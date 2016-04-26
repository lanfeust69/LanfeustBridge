using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.Entity;

using LanfeustBridge.Models;

namespace LanfeustBridge.Services
{
    public class DbTournamentsService : DbContext, ITournamentService
    {
        private int _nextId = 0;

        public DbSet<Tournament> Tournaments { get; set; }

        public IEnumerable<Tuple<int, string>> GetNames()
        {
            throw new NotImplementedException();
        }

        public int GetNextId()
        {
            return _nextId++;
        }

        public Tournament GetTournament(int id)
        {
            return Tournaments.FirstOrDefault(t => t.Id == id);
        }

        public Tournament SaveTournament(Tournament tournament)
        {
            var existing = GetTournament(tournament.Id);
            if (existing != null)
                Tournaments.Remove(tournament);
            else
                tournament.Id = GetNextId();
            Tournaments.Add(tournament);
            SaveChanges();
            return tournament;
        }

        public bool DeleteTournament(int id)
        {
            var toRemove = Tournaments.FirstOrDefault(t => t.Id == id);
            if (toRemove == null)
                return false;
            Tournaments.Remove(toRemove);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make Tournament.Id required
            modelBuilder.Entity<Tournament>()
                .Property(t => t.Id)
                .IsRequired();

            // Make Deal.Id required
            modelBuilder.Entity<Deal>()
                .Property(d => d.Id)
                .IsRequired();
        }
    }
}
