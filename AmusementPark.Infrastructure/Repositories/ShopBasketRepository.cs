using System.Data.OleDb;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;
using AmusementPark.Infrastructure.Data;

namespace AmusementPark.Infrastructure.Repositories
{

    /// Repository مربوط به جدول ShopBasket در Access.
    ///
    /// ستون‌های مورد استفاده:
    /// PlayerID
    /// AmusementName
    /// GameName
    /// GamePrice
    /// TotalGamePrice

    public class ShopBasketRepository
        : IShopBasketRepository
    {
        public void ReplacePlayerBasket(
            int playerId,
            IEnumerable<ShopBasket> items)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            using OleDbTransaction transaction =
                connection.BeginTransaction();

            try
            {
                // ابتدا سبد قبلی همین کاربر حذف می‌شود
                // تا با چند بار زدن پرداخت رکورد تکراری ایجاد نشود.

                const string deleteSql =
                    "DELETE FROM ShopBasket WHERE PlayerID = ?";

                using (OleDbCommand deleteCommand =
                    new(
                        deleteSql,
                        connection,
                        transaction))
                {
                    deleteCommand.Parameters.AddWithValue(
                        "@PlayerID",
                        playerId);

                    deleteCommand.ExecuteNonQuery();
                }

                const string insertSql = @"
                   INSERT INTO ShopBasket
(
    PlayerID,
    AmusementName,
    GameName,
    GamePrice,
    Quantity,
    TotalGamePrice
)
VALUES (?,?,?,?,?,?)";

                foreach (ShopBasket item in items)
                {
                    using OleDbCommand insertCommand =
                        new(
                            insertSql,
                            connection,
                            transaction);

                    insertCommand.Parameters.AddWithValue(
                        "@PlayerID",
                        playerId);

                    insertCommand.Parameters.AddWithValue(
                        "@AmusementName",
                        item.AmusementName ?? string.Empty);

                    insertCommand.Parameters.AddWithValue(
                        "@GameName",
                        item.GameName ?? string.Empty);

                    insertCommand.Parameters.AddWithValue(
                        "@GamePrice",
                        item.GamePrice);
                    insertCommand.Parameters.AddWithValue(
    "@Quantity",
    item.Quantity);

                    
                 
                    insertCommand.Parameters.AddWithValue(
                        "@TotalGamePrice",
                        item.TotalPrice);

                    insertCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
        }

        public void ClearPlayerBasket(
            int playerId)
        {
            using OleDbConnection connection =
                AccessConnection.GetConnection();

            connection.Open();

            const string sql =
                "DELETE FROM ShopBasket WHERE PlayerID = ?";

            using OleDbCommand command =
                new(
                    sql,
                    connection);

            command.Parameters.AddWithValue(
                "@PlayerID",
                playerId);

            command.ExecuteNonQuery();
        }
    }
}