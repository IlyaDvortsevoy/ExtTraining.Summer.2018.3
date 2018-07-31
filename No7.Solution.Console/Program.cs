using System.Reflection;

namespace No7.Solution.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("No7.Solution.Console.trades.txt");

            var handler = new TradeHandler(tradeStream);

            var records = handler.GetRecords();

            var repository = new TradeRecordRepository("TradeData");

            repository.Save(records);
        }
    }
}