using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class ProductRepository : BaseRepository<int, Product>, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Product newProduct(Product product)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO `Product` (name, description, quantity, price, status, CreatedDate, ModifiedDate, categoryID)
                                              VALUES (@v_name, @v_description, @v_quantity, @v_price, 'ACTIVE', NOW(), NOW(), @v_categoryID);";
                using (MySqlCommand command = new MySqlCommand(sqlCommand, SqlConnection))
                {
                    command.Parameters.AddWithValue("@v_name", product.name);
                    command.Parameters.AddWithValue("@v_description", product.description);
                    command.Parameters.AddWithValue("@v_quantity", product.quantity);
                    command.Parameters.AddWithValue("@v_price", product.price);
                    command.Parameters.AddWithValue("@v_categoryID", product.categoryID);

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
