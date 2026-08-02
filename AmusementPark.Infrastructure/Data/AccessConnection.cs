using System.Data.OleDb;
using System.IO;

namespace AmusementPark.Infrastructure.Data
{
    public class AccessConnection
    {
        public static OleDbConnection GetConnection()
        {
            string dbPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Database",
                "AmusementPark.accdb");

            string connectionString =
                $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;";

            return new OleDbConnection(connectionString);
        }
    }
}