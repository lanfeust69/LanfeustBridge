using System;
using System.Collections.Generic;
using System.Linq;

namespace LanfeustBridge.Models
{
    public class Individual12 : IMovement
    {
        private const int NB_PLAYERS = 12;

        public MovementDescription MovementDescription { get; } = new MovementDescription
        {
            Order = 3,
            Id = typeof(Individual12).Name.ToLower(),
            Name = $"Individual for {NB_PLAYERS} players",
            Description = "Only accepts 11 rounds, playing with each of the 11 other players",
            NbPlayers = NB_PLAYERS,
            MinRounds = 11,
            MaxRounds = 11
        };

        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var positions = new Position[NB_PLAYERS];
                for (int table = 0; table < 3; table++)
                {
                    var position = new Position { Table = table };
                    int firstDeal = round * nbDealsPerRound + 1;
                    int offset = table * nbDealsPerRound / 3;
                    position.Deals = Enumerable.Range(0, nbDealsPerRound).Select(i => firstDeal + (offset + i) % nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 11 : (table == 1 ? (round + 1) % 11 : (round + 3) % 11);
                    position.South = table == 0 ? round : (table == 1 ? (round + 8) % 11 : (round + 2) % 11);
                    position.East = table == 0 ? (round + 5) % 11 : (table == 1 ? (round + 9) % 11 : (round + 4) % 11);
                    position.West = table == 0 ? (round + 7) % 11 : (table == 1 ? (round + 6) % 11 : (round + 10) % 11);
                    positions[position.North] = position;
                    positions[position.South] = position;
                    positions[position.East] = position;
                    positions[position.West] = position;
                }
                allPositions[round] = positions;
            }
            return allPositions;
        }

        public Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            int nbDeals = nbRounds * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
            {
                var deal = Deal.CreateDeal(i + 1, 3, Deal.ComputeDealer(i % 8 + 1), Deal.ComputeVulnerability(i % 8 + 1));
                deal.Scores[0].Round = deal.Scores[1].Round = deal.Scores[2].Round = i / nbDealsPerRound;
                deal.Scores[0].Table = 0;
                deal.Scores[1].Table = 1;
                deal.Scores[2].Table = 2;
                deals[i] = deal;
            }
            return deals;
        }

        public MovementValidation Validate(int nbTables, int nbRounds)
        {
            var reasons = new List<string>();
            // ignore nbTables, as we have a fixed number of players, and the GUI won't display it (nor allow to change it)
            if (nbRounds != 11)
                reasons.Add("Only 11 rounds are allowed for 12-player individuals");
            return new MovementValidation { IsValid = reasons.Count == 0, Reason = string.Join(" ; ", reasons) };
        }

        private void CheckValidity(int nbTables, int nbRounds)
        {
            var validity = Validate(nbTables, nbRounds);
            if (!validity.IsValid)
                throw new NotSupportedException(validity.Reason);
        }
    }
}
