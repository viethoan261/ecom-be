using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class CartDetailRepository : BaseRepository<int, CartDetail>, ICartDetailRepository
    {
        public CartDetailRepository(IConfiguration configuration) : base(configuration)
        {
           
        }

        public CartDetail getCartDetail(int productID, string size, string color, int cartID)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"SELECT * from `CartDetail` where productID = @v_productID and size = @v_size and color = @v_color and cartID = @v_cartID and status = 'ACTIVE';";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_productID", productID);
                parameters.Add("@v_size", size);
                parameters.Add("@v_color", color);
                parameters.Add("@v_cartID", cartID);
                //Trả dữ liệu về client
                var cartDetail = SqlConnection.QueryFirstOrDefault<CartDetail>(sqlCommand, parameters);
                SqlConnection.Close();
                return cartDetail;
            }
        }
    }
}
