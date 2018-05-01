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
            Description = "Only accepts 14 rounds : 2 rounds playing with each of the 7 other players",
            NbPlayers = NB_PLAYERS,
            MinRounds = 14,
            MaxRounds = 14
        };

        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            // each "player" round corresponds to 2 rounds where only deals move
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var playerRound = round / 2;
                var positions = new Position[NB_PLAYERS];
                for (int table = 0; table < 2; table++)
                {
                    var position = new Position { Table = table + 1 };
                    int firstDeal = (round - round % 2 + (round + table) % 2) * nbDealsPerRound + 1;
                    position.Deals = Enumerable.Range(firstDeal, nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 7 : (playerRound + 4) % 7;
                    position.South = table == 0 ? playerRound : (playerRound + 6) % 7;
                    position.East = table == 0 ? (playerRound + 5) % 7 : (playerRound + 2) % 7;
                    position.West = table == 0 ? (playerRound + 1) % 7 : (playerRound + 3) % 7;
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
                deals[i] = Deal.CreateDeal(i + 1, nbRounds, Deal.ComputeDealer(i % 8 + 1), Deal.ComputeVulnerability(i % 8 + 1));
            return deals;
        }

        public MovementValidation Validate(int nbTables, int nbRounds)
        {
            var reasons = new List<string>();
            // ignore nbTables, as we have a fixed number of players, and the GUI won't display it (nor allow to change it)
            if (nbRounds != 14)
                reasons.Add("Only 14 rounds are allowed for 8-player individuals");
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
