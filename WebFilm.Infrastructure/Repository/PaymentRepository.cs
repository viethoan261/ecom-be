using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Payment;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class PaymentRepository : BaseRepository<int, Payment>, IPaymentRepository
    {
        public PaymentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Payment getByOrderID(int orderID)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"SELECT * from `Payment` where orderID = @v_orderID;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_orderID", orderID);
                //Trả dữ liệu về client
                var res = SqlConnection.QueryFirstOrDefault<Payment>(sqlCommand, parameters);
                SqlConnection.Close();
                return res;
            }
        }

        public int newPayment(int orderID, float amount)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO payment (OrderID, Amount, PaymentDate, CreatedDate, ModifiedDate)
                                              VALUES (@v_OrderID, @v_Amount, NOW(), NOW(),  NOW());";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_OrderID", orderID);
                parameters.Add("v_Amount", amount);
                var res = SqlConnection.Execute(sqlCommand, parameters);
                if (res > 0)
                {
                    SqlConnection.Close();
                    return res;
                }
                else
                {
                    SqlConnection.Close();
                    return res;
                }
            }
        }
    }
}
