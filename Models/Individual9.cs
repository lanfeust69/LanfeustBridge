using System;
using System.Collections.Generic;
using System.Linq;

namespace LanfeustBridge.Models
{
    public class Individual9 : IMovement
    {
        private const int NB_PLAYERS = 9;

        public MovementDescription MovementDescription { get; } = new MovementDescription
        {
            Order = 4,
            Id = typeof(Individual9).Name.ToLower(),
            Name = $"Individual for {NB_PLAYERS} players",
            Description = "Only accepts 27 rounds : 3 rounds playing with each of the 8 other players, plus a bye",
            NbPlayers = NB_PLAYERS,
            MinRounds = 27,
            MaxRounds = 27
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
                for (int table = 0; table < 2; table++)
                {
                    var position = new Position { Table = table + 1 };
                    int firstDeal = (round - round % 3 + (round + table) % 3) * nbDealsPerRound + 1;
                    position.Deals = Enumerable.Range(firstDeal, nbDealsPerRound).ToArray();
                    position.North = table == 0 ? (playerRound / 3) * 3 + (playerRound + 1) % 3 :
                        ((playerRound / 3 + 1) % 3) * 3 + (playerRound + 1) % 3;
                    position.South = table == 0 ? (playerRound / 3) * 3 + (playerRound + 2) % 3 :
                        ((playerRound / 3 + 2) % 3) * 3 + (playerRound + 2) % 3;
                    position.East = table == 0 ? (playerRound + 3) % 9 :
                        ((playerRound / 3 + 1) % 3) * 3 + (playerRound + 2) % 3;
                    position.West = table == 0 ? (playerRound + 6) % 9 :
                        ((playerRound / 3 + 2) % 3) * 3 + (playerRound + 1) % 3;
                    positions[position.North] = position;
                    positions[position.South] = position;
                    positions[position.East] = position;
                    positions[position.West] = position;
                }
                // bye is considered being at all places on pseudo-table 3
                var bye = new Position
                {
                    Table = 3, Deals = new int[0],
                    North = playerRound, South = playerRound, East = playerRound, West = playerRound
                };
                positions[playerRound] = bye;
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
            if (nbRounds != 27)
                reasons.Add("Only 27 rounds are allowed for 9-player individuals");
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
