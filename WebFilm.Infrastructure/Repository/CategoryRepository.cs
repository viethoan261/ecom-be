using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class CategoryRepository : BaseRepository<int, Category>, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Category inActive(int id)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"Update `Category` set status = 'INACTIVE', ModifiedDate = NOW() where id = @v_id;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_id", id);
                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);

                if (affectedRows > 0)
                {
                    var res = SqlConnection.QueryFirstOrDefault<Category>("SELECT * FROM `Category` WHERE id = @v_id", parameters);
                    SqlConnection.Close();
                    return res;
                }

                //Trả dữ liệu về client
                SqlConnection.Close();
                return null;
            }
        }

        public Category newCategory(CategoryDTO dto)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"INSERT INTO `Category` (name, description, status, categoryParentID, CreatedDate, ModifiedDate)
                                              VALUES (@v_name, @v_description, 'ACTIVE', @v_categoryParentID, NOW(), NOW());";
                using (MySqlCommand command = new MySqlCommand(sqlCommand, SqlConnection))
                {
                    command.Parameters.AddWithValue("@v_name", dto.name);
                    command.Parameters.AddWithValue("@v_description", dto.description);
                    command.Parameters.AddWithValue("@v_categoryParentID", dto.categoryParentID);

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

        public Category updateCategory(int id, CategoryDTO dto)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = $@"Update `Category` set name = @v_name, description = @v_description,
categoryParentID = @v_categoryParentID, ModifiedDate = NOW() where id = @v_id;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_name", dto.name);
                parameters.Add("v_description", dto.description);
                parameters.Add("v_id", id);
                parameters.Add("v_categoryParentID", dto.categoryParentID);
                var affectedRows = SqlConnection.Execute(sqlCommand, parameters);

                if (affectedRows > 0)
                {
                    var res = SqlConnection.QueryFirstOrDefault<Category>("SELECT * FROM `Category` WHERE id = @v_id", parameters);
                    SqlConnection.Close();
                    return res;
                }

                //Trả dữ liệu về client
                SqlConnection.Close();
                return null;
            }
        }
    }
}
