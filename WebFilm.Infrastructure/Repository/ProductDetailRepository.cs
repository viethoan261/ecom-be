using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Payment;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class ProductDetailRepository : BaseRepository<int, ProductDetail>, IProductDetailRepository
    {
        public ProductDetailRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public ProductDetail getProductDetail(int productID, string size, string color)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"SELECT * from `ProductDetail` where productID = @v_productID and size = @v_size and color = @v_color and status = 'ACTIVE';";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_productID", productID);
                parameters.Add("@v_size", size);
                parameters.Add("@v_color", color);
                //Trả dữ liệu về client
                var productDetail = SqlConnection.QueryFirstOrDefault<ProductDetail>(sqlCommand, parameters);
                SqlConnection.Close();
                return productDetail;
            }
        }
    }
}
