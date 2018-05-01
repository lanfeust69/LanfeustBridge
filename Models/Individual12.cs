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
            Description = "Only accepts 33 rounds : 3 rounds playing with each of the 11 other players",
            NbPlayers = NB_PLAYERS,
            MinRounds = 33,
            MaxRounds = 33
        };

        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            // each "player" round corresponds to 3 rounds where only deals move
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var playerRound = round / 3;
                var positions = new Position[NB_PLAYERS];
                for (int table = 0; table < 3; table++)
                {
                    var position = new Position { Table = table + 1 };
                    int firstDeal = (round - (round % 3) + ((round % 3) + table) % 3) * nbDealsPerRound + 1;
                    position.Deals = Enumerable.Range(firstDeal, nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 11 : (table == 1 ? (playerRound + 1) % 11 : (playerRound + 3) % 11);
                    position.South = table == 0 ? playerRound : (table == 1 ? (playerRound + 8) % 11 : (playerRound + 2) % 11);
                    position.East = table == 0 ? (playerRound + 5) % 11 : (table == 1 ? (playerRound + 9) % 11 : (playerRound + 4) % 11);
                    position.West = table == 0 ? (playerRound + 7) % 11 : (table == 1 ? (playerRound + 6) % 11 : (playerRound + 10) % 11);
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
            if (nbRounds != 33)
                reasons.Add("Only 33 rounds are allowed for 12-player individuals");
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
