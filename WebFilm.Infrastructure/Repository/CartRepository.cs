using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Interfaces.Repository;
using static Dapper.SqlMapper;

namespace WebFilm.Infrastructure.Repository
{
    public class CartRepository : BaseRepository<int, Cart>, ICartRepository
    {
        public CartRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Cart getCart(int customerID)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"SELECT * from `Cart` where customerID = @v_customerID and status = 'ACTIVE';";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_customerID", customerID);
                //Trả dữ liệu về client
                var cart = SqlConnection.QueryFirstOrDefault<Cart>(sqlCommand, parameters);
                SqlConnection.Close();
                return cart;
            }
        }

        public Cart newCart(int customerID)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO `Cart` (customerID, CreatedDate, ModifiedDate, status)
                                              VALUES (@v_customerID, NOW(), NOW(), 'ACTIVE');";
                using (MySqlCommand command = new MySqlCommand(sqlCommand, SqlConnection))
                {
                    command.Parameters.AddWithValue("@v_customerID", customerID);

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
