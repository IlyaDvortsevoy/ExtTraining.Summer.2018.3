using System.Collections.Generic;
using System.IO;

namespace No7.Solution
{
    public class TradeHandler
    {
        private readonly float LotSize = 100000f;
        // Field for DI
        private readonly Stream _stream;

        // DI through constructor
        public TradeHandler(Stream stream)
        {
            _stream = stream;
        }

        // SRP applying
        public IEnumerable<TradeRecord> GetRecords()
        {
            var trades = new List<TradeRecord>();

            int lineCount = 1;
            foreach (var line in GetLines())
            {
                string[] fields = line.Split(new char[] { ',' });

                if (fields.Length != 3)
                {
                    System.Console.WriteLine("WARN: Line {0} malformed. Only {1} field(s) found.", lineCount, fields.Length);
                    continue;
                }

                if (fields[0].Length != 6)
                {
                    System.Console.WriteLine("WARN: Trade currencies on line {0} malformed: '{1}'", lineCount, fields[0]);
                    continue;
                }

                if (!int.TryParse(fields[1], out var tradeAmount))
                {
                    System.Console.WriteLine("WARN: Trade amount on line {0} not a valid integer: '{1}'", lineCount, fields[1]);
                }

                if (!decimal.TryParse(fields[2], out var tradePrice))
                {
                    System.Console.WriteLine("WARN: Trade price on line {0} not a valid decimal: '{1}'", lineCount, fields[2]);
                }

                var sourceCurrencyCode = fields[0].Substring(0, 3);
                var destinationCurrencyCode = fields[0].Substring(3, 3);

                var trade = new TradeRecord
                {
                    SourceCurrency = sourceCurrencyCode,
                    DestinationCurrency = destinationCurrencyCode,
                    Lots = tradeAmount / LotSize,
                    Price = tradePrice
                };

                trades.Add(trade);

                lineCount++;
            }

            return trades;
        }

        // SRP applying
        private IEnumerable<string> GetLines()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(_stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}