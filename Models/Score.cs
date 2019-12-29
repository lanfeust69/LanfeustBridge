using System.Text.Json.Serialization;

namespace LanfeustBridge.Models
{
    public class Score
    {
        public int DealId { get; set; }

        public string Vulnerability { get; set; } = default!;

        public int Round { get; set; }

        public bool Entered { get; set; }

        public Players Players { get; set; } = default!;

        public Contract Contract { get; set; } = default!;

        public int Tricks { get; set; }

        [JsonPropertyName("score")]
        public int BridgeScore { get; set; }

        [JsonPropertyName("nsResult")]
        public double NSResult { get; set; }

        [JsonPropertyName("ewResult")]
        public double EWResult { get; set; }

        public int ComputeBridgeScore()
        {
            if (Contract.Level == 0)
                return 0;
            int sign = Contract.Declarer == "N" || Contract.Declarer == "S" ? 1 : -1;
            bool vulnerable = Vulnerability == "Both" || (Vulnerability != "Both" && Vulnerability.Contains(Contract.Declarer));
            var result = Tricks - Contract.Level - 6;
            if (result < 0)
            {
                if (!Contract.Doubled && !Contract.Redoubled)
                    return result * sign * (vulnerable ? 100 : 50);
                int baseResult = sign * (result * 300 + 100);
                if (!vulnerable)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (result < -i)
                            baseResult += sign * 100;
                    }
                }
                return baseResult * (Contract.Redoubled ? 2 : 1);
            }
            var trickValue = Contract.Suit == Suit.Clubs || Contract.Suit == Suit.Diamonds ? 20 : 30;
            var contractValue = trickValue * Contract.Level + (Contract.Suit == Suit.NoTrump ? 10 : 0);
            if (Contract.Doubled)
                contractValue *= 2;
            if (Contract.Redoubled)
                contractValue *= 4;
            var sum = contractValue;
            if (contractValue >= 100)
                sum += vulnerable ? 500 : 300;
            else
                sum += 50;
            if (Contract.Level == 7)
                sum += vulnerable ? 1500 : 1000;
            if (Contract.Level == 6)
                sum += vulnerable ? 750 : 500;
            if (Contract.Doubled)
                sum += 50;
            if (Contract.Redoubled)
                sum += 100;

            var overtrickValue = trickValue;
            if (Contract.Doubled || Contract.Redoubled)
            {
                overtrickValue = vulnerable ? 200 : 100;
                if (Contract.Redoubled)
                    overtrickValue *= 2;
            }
            sum += result * overtrickValue;

            return sign * sum;
        }

        public bool Validate()
        {
            if (!Entered)
                return false;
            switch (Vulnerability)
            {
                case "None": case "NS": case "EW": case "Both":
                    break;
                default:
                    return false;
            }
            return BridgeScore == ComputeBridgeScore();
        }
    }
}
