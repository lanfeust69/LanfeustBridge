using System;
using System.Collections.Generic;
using System.Linq;

namespace LanfeustBridge.Models
{
    public class Individual8 : IMovement
    {
        private const int NB_PLAYERS = 8;

        public MovementDescription MovementDescription { get; } = new MovementDescription
        {
            Order = 5,
            Id = typeof(Individual8).Name.ToLower(),
            Name = $"Individual for {NB_PLAYERS} players",
            Description = "Only accepts 7 rounds, playing with each of the 7 other players",
            NbPlayers = NB_PLAYERS,
            MinRounds = 7,
            MaxRounds = 7
        };

        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            // each "player" round corresponds to 2 rounds where only deals move
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var positions = new Position[NB_PLAYERS];
                for (int table = 0; table < 2; table++)
                {
                    var position = new Position { Table = table };
                    int firstDeal = round * nbDealsPerRound + 1;
                    int offset = table * nbDealsPerRound / 2;
                    position.Deals = Enumerable.Range(0, nbDealsPerRound).Select(i => firstDeal + (offset + i) % nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 7 : (round + 4) % 7;
                    position.South = table == 0 ? round : (round + 6) % 7;
                    position.East = table == 0 ? (round + 5) % 7 : (round + 2) % 7;
                    position.West = table == 0 ? (round + 1) % 7 : (round + 3) % 7;
                    positions[position.North] = position;
                    positions[position.South] = position;
                    positions[position.East] = position;
                    positions[position.West] = position;
                }
                allPositions[round] = positions;
            }
            return allPositions;
        }

        public Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound, int nbBoards)
        {
            CheckValidity(nbTables, nbRounds);
            if (nbDealsPerRound > nbBoards)
                throw new NotSupportedException($"Need at least {nbDealsPerRound} boards");

            int nbDeals = nbRounds * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
            {
                var deal = Deal.CreateDeal(i + 1, 2, Deal.ComputeDealer(i % nbBoards + 1), Deal.ComputeVulnerability(i % nbBoards + 1));
                deal.Scores[0].Round = deal.Scores[1].Round = i / nbDealsPerRound;
                deal.Scores[0].Table = 0;
                deal.Scores[1].Table = 1;
                deals[i] = deal;
            }
            return deals;
        }

        public MovementValidation Validate(int nbTables, int nbRounds)
        {
            var reasons = new List<string>();
            // ignore nbTables, as we have a fixed number of players, and the GUI won't display it (nor allow to change it)
            if (nbRounds != 7)
                reasons.Add("Only 7 rounds are allowed for 8-player individuals");
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
