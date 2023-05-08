using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class OrderRepository : BaseRepository<int, Order>, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Order newOrder(int customerID, float price, string address, string method)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO `Order` (customerID, orderDate,staffID, price, status, CreatedDate, ModifiedDate, address, method)
                                              VALUES (@v_customerID, NOW(),0, @v_price, 'PENDING', NOW(), NOW(), @v_address, @v_method);";
                using (MySqlCommand command = new MySqlCommand(sqlCommand, SqlConnection))
                {
                    command.Parameters.AddWithValue("@v_customerID", customerID);
                    command.Parameters.AddWithValue("@v_price", price);
                    command.Parameters.AddWithValue("@v_address", address);
                    command.Parameters.AddWithValue("@v_method", method);

                    SqlConnection.Open();
                    command.ExecuteNonQuery();
                    int insertedId = (int)command.LastInsertedId;
                    // Lấy lại đối tượng vừa chèn
                    var insertedObject = GetByID(insertedId);
                    SqlConnection.Close();
                    return insertedObject;
                }
            }
        }
    }
}
