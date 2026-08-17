using System.Data.OleDb;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;
using AmusementPark.Infrastructure.Data;

namespace AmusementPark.Infrastructure.Repositories
{
    /// <summary>
    /// پیاده‌سازی Repository مربوط به Player.
    ///
    /// این کلاس تنها جایی است که Application
    /// مستقیماً با دیتابیس Access ارتباط دارد.
    /// </summary>
    public class PlayerRepository : IPlayerRepository
    {
        public List<Player> GetAll()
        {
            List<Player> players = new();

            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string query =
                "SELECT PlayerID, PlayerName, PlayerMobile FROM Player";

            using OleDbCommand command =
                new(query, connection);

            using OleDbDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                players.Add(MapPlayer(reader));
            }

            return players;
        }

        public Player? GetById(int id)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string query =
                "SELECT PlayerID, PlayerName, PlayerMobile ,IsAdmin " +
                "FROM Player WHERE PlayerID=?";

            using OleDbCommand command =
                new(query, connection);

            command.Parameters.AddWithValue(
                "@PlayerID",
                id);

            using OleDbDataReader reader =
                command.ExecuteReader();

            if (reader.Read())
            {
                return MapPlayer(reader);
            }

            return null;
        }

        public Player? GetByMobile(string mobile)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string query =
                "SELECT PlayerID, PlayerName, PlayerMobile, IsAdmin " +
                "FROM Player WHERE PlayerMobile=?";

            using OleDbCommand command =
                new(query, connection);

            command.Parameters.AddWithValue(
                "@PlayerMobile",
                mobile);

            using OleDbDataReader reader =
                command.ExecuteReader();

            if (reader.Read())
            {
                return MapPlayer(reader);
            }

            return null;
        }

        /// <summary>
        /// پیدا کردن کاربر بر اساس نام کاربری.
        /// </summary>
        public Player? GetByName(string playerName)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string query =
                "SELECT PlayerID, PlayerName, PlayerMobile, IsAdmin " +
                "FROM Player WHERE PlayerName=?";

            using OleDbCommand command =
                new(query, connection);

            command.Parameters.AddWithValue(
                "@PlayerName",
                playerName);

            using OleDbDataReader reader =
                command.ExecuteReader();

            if (reader.Read())
            {
                return MapPlayer(reader);
            }

            return null;
        }

        /// <summary>
        /// پیدا کردن کاربر با نام کاربری و شماره موبایل.
        ///
        /// این متد برای Login استفاده می‌شود.
        /// </summary>
        public Player? GetByNameAndMobile(
            string playerName,
            string mobile)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string query =
                "SELECT PlayerID, PlayerName, PlayerMobile, IsAdmin " +
                "FROM Player " +
                "WHERE PlayerName=? AND PlayerMobile=?";

            using OleDbCommand command =
                new(query, connection);

            command.Parameters.AddWithValue(
                "@PlayerName",
                playerName);

            command.Parameters.AddWithValue(
                "@PlayerMobile",
                mobile);

            using OleDbDataReader reader =
                command.ExecuteReader();

            if (reader.Read())
            {
                return MapPlayer(reader);
            }

            return null;
        }

        /// <summary>
        /// ثبت Player جدید در جدول Player.
        ///
        /// این همان جایی است که:
        ///
        /// username
        /// +
        /// mobile
        ///
        /// واقعاً داخل Access Database ذخیره می‌شوند.
        /// </summary>
        public void Add(Player player)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string query =
                "INSERT INTO Player " +
                "(PlayerName, PlayerMobile, IsAdmin) " +
                "VALUES (?, ?, ?)";

            using OleDbCommand command =
                new(query, connection);

            command.Parameters.AddWithValue(
                "@PlayerName",
                player.PlayerName);

            command.Parameters.AddWithValue(
                "@PlayerMobile",
                player.PlayerMobile);

            command.Parameters.AddWithValue(
                "@IsAdmin",
                false);

            command.ExecuteNonQuery();
        }

        public void Update(Player player)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string query =
                "UPDATE Player " +
                "SET PlayerName=?, PlayerMobile=? " +
                "WHERE PlayerID=?";

            using OleDbCommand command =
                new(query, connection);

            command.Parameters.AddWithValue(
                "@PlayerName",
                player.PlayerName);

            command.Parameters.AddWithValue(
                "@PlayerMobile",
                player.PlayerMobile);

            command.Parameters.AddWithValue(
                "@PlayerID",
                player.PlayerID);

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string query =
                "DELETE FROM Player WHERE PlayerID=?";

            using OleDbCommand command =
                new(query, connection);

            command.Parameters.AddWithValue(
                "@PlayerID",
                id);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// تبدیل رکورد دیتابیس به Entity.
        /// </summary>
        private static Player MapPlayer(OleDbDataReader reader)
        {
            return new Player
            {
                PlayerID = Convert.ToInt32(reader["PlayerID"]),

                PlayerName = reader["PlayerName"]?.ToString(),

                PlayerMobile = reader["PlayerMobile"]?.ToString(),

                IsAdmin = reader["IsAdmin"] != DBNull.Value &&
                       Convert.ToBoolean(reader["IsAdmin"])
            };
        }
    }
}