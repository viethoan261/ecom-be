using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Newtonsoft.Json;
using WebFilm.Core.Enitites;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.Statistic;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<int, User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #region Method
        public User GetUserByID(Guid userID)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT * FROM User WHERE UserID = @v_UserID";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserID", userID);
                var user = SqlConnection.QueryFirstOrDefault<User>(sqlCommand, parameters);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return user;
            }
        }

        public User Signup(UserDto user)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO user (UserName, FullName, Password, Email, Status, Role,CreatedDate, ModifiedDate, Phone)
                                              VALUES (@v_UserName, @v_FullName, @v_Password, @v_Email, 1, @v_Role, NOW(),  NOW(), @v_Phone);";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserName", user.userName);
                parameters.Add("v_FullName", user.fullName);
                parameters.Add("v_Password", user.password);
                parameters.Add("v_Email", user.email);
                parameters.Add("v_Role", user.role);
                parameters.Add("v_Phone", user.phone);
                var res = SqlConnection.Execute(sqlCommand, parameters);
                if (res > 0)
                {
                    var userRes = SqlConnection.QueryFirstOrDefault<User>("SELECT * FROM user WHERE Email = @v_Email", parameters);
                    SqlConnection.Close();
                    return userRes;
                } else
                {
                    SqlConnection.Close();
                    return null;
                }               
            }
        }

        public User Login(string username)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $"SELECT * FROM user WHERE Username = @v_Username";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_Username", username);
                var res = SqlConnection.QueryFirstOrDefault<User>(sqlCommand, parameters);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return res;
            }
        }

        public bool CheckDuplicateEmail(string email)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCheck = "SELECT * FROM User WHERE Email = @v_Email";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_Email", email);

                var res = SqlConnection.QueryFirstOrDefault<User>(sql: sqlCheck, param: parameters);
                if (res != null)
                {
                    return true;
                }
                return false;
            }
        }

        public bool CheckDuplicateUsername(string username)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCheck = "SELECT * FROM User WHERE Username = @v_Username";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_Username", username);

                var res = SqlConnection.QueryFirstOrDefault<User>(sql: sqlCheck, param: parameters);
                if (res != null)
                {
                    return true;
                }
                return false;
            }
        }

        public bool ActiveUser(string email)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCheck = "UPDATE user SET Status = 2 WHERE Email = @v_Email";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_Email", email);

                var res = SqlConnection.Execute(sql: sqlCheck, param: parameters);
                
                return true;
            }
        }

        public bool ChangePassword(string email, string newPass)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCheck = "UPDATE user u SET Password = @v_NewPass WHERE u.Email = @v_Email;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_Email", email);
                parameters.Add("@v_NewPass", newPass);

                var res = SqlConnection.Execute(sql: sqlCheck, param: parameters);
                if(res > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool AddTokenReset(User user)
        {
           
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCheck = "UPDATE User u SET u.PasswordResetToken = @v_PasswordResetToken, u.ResetTokenExpires = @v_ResetTokenExpires WHERE u.ID = @v_UserID;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_UserID", user.id);
                parameters.Add("@v_PasswordResetToken", user.passwordResetToken);
                parameters.Add("@v_ResetTokenExpires", user.resetTokenExpires);

                var res = SqlConnection.Execute(sql: sqlCheck, param: parameters);
                if (res > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<User> GetUserByTokenReset(string token)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT * FROM User WHERE PasswordResetToken = @v_PasswordResetToken";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_PasswordResetToken", token);
                var user = await SqlConnection.QueryFirstOrDefaultAsync<User>(sqlCommand, parameters);
                //Trả dữ liệu về client
                SqlConnection.Close();
                return user;
            }
        }

        public User getUserByUsername(string username)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT * FROM User WHERE  UserName = @v_UserName";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_UserName", username);
                var user = SqlConnection.QueryFirstOrDefault<User>(sqlCommand, parameters);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return user;
            }
        }

        public User GetUserByEmail(string email)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT * FROM User WHERE  Email = @v_Email";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_Email", email);
                var user = SqlConnection.QueryFirstOrDefault<User>(sqlCommand, parameters);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return user;
            }
        }

        public User ChangeProfile(int id, UserProfileDTO dto)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCheck = "UPDATE User u SET u.Fullname = @v_fullname, u.Email = @v_email, u.Phone = @v_phone, u.Address = @v_address, u.City = @v_city, u.District = @v_district, u.Ward = @v_ward  WHERE u.ID = @v_UserID;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@v_UserID", id);
                parameters.Add("@v_fullname", dto.fullName);
                parameters.Add("@v_email", dto.email);
                parameters.Add("@v_phone", dto.phone);
                parameters.Add("@v_address", dto.address);
                parameters.Add("@v_city", dto.city);
                parameters.Add("@v_district", dto.district);
                parameters.Add("@v_ward", dto.ward);

                var res = SqlConnection.Execute(sql: sqlCheck, param: parameters);
                if (res > 0)
                {
                    var resUser = SqlConnection.QueryFirstOrDefault<User>("SELECT * FROM `User` WHERE id = @v_UserID", parameters);
                    SqlConnection.Close();
                    return resUser;
                }
                return null;
            }
        }

        public StatisticUser userStat()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT count(*) as totalUser, ( select count(DISTINCT `order`.customerID) from `order`) as totalUserOrdered from user;";
                var res = SqlConnection.QueryFirstOrDefault<StatisticUser>(sqlCommand);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return res;
            }
        }

        public StatisticProduct productStat()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT count(*) as totalProduct, ( select count(*) from `product` where product.status = 'ACTIVE') as totalActiveProduct from product;\r\n";
                var res = SqlConnection.QueryFirstOrDefault<StatisticProduct>(sqlCommand);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return res;
            }
        }

        public List<StatisticOrder> orderStat()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "select `order`.status as status, count(*) as totalOrder \r\nfrom `order`\r\ngroup by `order`.`Status`;";
                var res = SqlConnection.Query<StatisticOrder>(sqlCommand);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return res.ToList();
            }
        }

        public List<StatisticTurnover> statisticTurnovers()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = "SELECT DATE(paymentdate) AS paymentDate, SUM(amount) AS totalAmount\r\nFROM payment\r\nGROUP BY DATE(paymentdate);";
                var res = SqlConnection.Query<StatisticTurnover>(sqlCommand);

                //Trả dữ liệu về client
                SqlConnection.Close();
                return res.ToList();
            }
        }
        #endregion
    }
}
