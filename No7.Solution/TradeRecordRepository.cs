using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace No7.Solution
{
    // Repository class for incapsulation of DA layer
    public class TradeRecordRepository
    {
        private readonly string _connectionString;

        public TradeRecordRepository(string connectionName)
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[connectionName];

            _connectionString = settings.ConnectionString;
        }

        public void Save(IEnumerable<TradeRecord> trades)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo.Insert_Trade";
                        command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                        command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                        command.Parameters.AddWithValue("@lots", trade.Lots);
                        command.Parameters.AddWithValue("@price", trade.Price);

                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                connection.Close();
            }

            System.Console.WriteLine("INFO: {0} trades processed", trades.ToArray().Length);
        }
    }
}