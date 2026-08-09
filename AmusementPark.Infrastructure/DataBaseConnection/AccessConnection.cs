using System.Data.OleDb;

namespace AmusementPark.Infrastructure.Data
{
    public static class AccessConnection
    {
        public static OleDbConnection GetConnection()
        {
            string dbPath = Path.Combine(
                AppContext.BaseDirectory,
                "DataBase",
                "AmusementParkDB.accdb");

            string connectionString =
                $@"Provider=Microsoft.ACE.OLEDB.12.0;
                   Data Source={dbPath};
                   Persist Security Info=False;";

            return new OleDbConnection(connectionString);
        }
    }
}