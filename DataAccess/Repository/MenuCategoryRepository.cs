using System.Collections.Generic;
using RestoAppAPI.Modal;
using System.Data;
using System.Data.SqlClient;
using System;
using Microsoft.Extensions.Configuration;

namespace RestoAppAPI.Repository
{
   public class MenuCategoryRepository : IMenuCategoryRepository
    {
        private readonly IConfiguration _configuration;
      
        public MenuCategoryRepository(IConfiguration configuration)
        {
            this._configuration = configuration;            
        }

        public MenuCategoryModal GetById(int Id)
        {
            
            MenuCategoryModal menuCategory = new MenuCategoryModal();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand("select ID,Name,Description From MenuCategory where Id=@Id and IsDeleted=0", connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    command.Parameters.AddWithValue("@Id", Id);                    

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuCategory.ID = (int)reader["ID"];
                            menuCategory.Name = (string)reader["Name"];
                            menuCategory.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : null;                            
                        }
                    }
                }
            }
            return menuCategory;
        }

        public int GetAllCount()
        {
            var count=0;
             using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand("select Count(*) count from MenuCategory where IsDeleted=0", connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                       

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                       

                           count= (int)reader["count"];
                           
                        }
                    }
                }
            }
            return count;
        }

        public List<MenuCategoryModal> GetByPagination(int Pagesize=10, int PageNumber=1)
        {
            
            List<MenuCategoryModal> menuCategories = new List<MenuCategoryModal>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand("Get_MenuCategories", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    command.Parameters.AddWithValue("@PageSize", Pagesize);
                    command.Parameters.AddWithValue("@PageNumber", PageNumber);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MenuCategoryModal menuCategory = new MenuCategoryModal();

                            menuCategory.ID = (int)reader["ID"];
                            menuCategory.Name = (string)reader["Name"];
                            menuCategory.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : null;
                            menuCategories.Add(menuCategory);
                        }
                    }
                }
            }
            return menuCategories;
        }



        public List<MenuCategoryModal> GetBySearchText(string searchText)
        {
            
        List<MenuCategoryModal> menuCategories = new List<MenuCategoryModal>();

        using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
        {
            using (SqlCommand command = new SqlCommand("Get_MenuCategories_search", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                command.Parameters.AddWithValue("SearchText", searchText);                

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MenuCategoryModal menuCategory = new MenuCategoryModal();

                        menuCategory.ID = (int)reader["ID"];
                        menuCategory.Name = (string)reader["Name"];
                        menuCategory.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : null;
                        menuCategories.Add(menuCategory);
                    }
                }
            }
        }
        return menuCategories;
        }

        public string Save(MenuCategoryModal menuCategoryModal)
        {
             using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Insert_Update_MenuCategory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        command.Parameters.AddWithValue("@ID", menuCategoryModal.ID);
                        command.Parameters.AddWithValue("@Name", menuCategoryModal.Name);
                        command.Parameters.AddWithValue("@Description", menuCategoryModal.Description);
                        command.Parameters.AddWithValue("@UserId", 1);
                        command.Parameters.AddWithValue("@IsDeleted", menuCategoryModal.IsDeleted);                        

                        // Output parameter
                        SqlParameter errorMessageParam = command.Parameters.Add("@ErrorMessage", SqlDbType.NVarChar, 100);
                        errorMessageParam.Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        return(string)errorMessageParam.Value;
                    
                    }
                }
           
        }
    }
}