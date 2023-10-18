using System.Collections.Generic;
using RestoAppAPI.Modal;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace RestoAppAPI.Repository
{
    public class MenuCategoryDapperRepository : IMenuCategoryRepository
    {
        private readonly IConfiguration _configuration;

        public MenuCategoryDapperRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public List<MenuCategoryModal> GetByPagination(int Pagesize = 10, int PageNumber = 1)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@PageSize", Pagesize);
                parameters.Add("@PageNumber", PageNumber);

                return connection.Query<MenuCategoryModal>("Get_MenuCategories", parameters, commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public List<MenuCategoryModal> GetBySearchText(string searchText)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@SearchText", searchText);

                return connection.Query<MenuCategoryModal>("Get_MenuCategories_search", parameters, commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public string Save(MenuCategoryModal menuCategoryModal)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@ID", menuCategoryModal.ID);
                parameters.Add("@Name", menuCategoryModal.Name);
                parameters.Add("@Description", menuCategoryModal.Description);
                parameters.Add("@UserId", 1);
                parameters.Add("@IsDeleted", menuCategoryModal.IsDeleted);

                parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                connection.Execute("Insert_Update_MenuCategory", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<string>("@ErrorMessage");
            }
        }
    }
}
