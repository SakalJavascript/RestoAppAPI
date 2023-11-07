using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using RestoAppAPI.Helpers;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IConfiguration _configuration;
      
        public MenuRepository(IConfiguration configuration)
        {
            this._configuration = configuration;            
        }
        public List<MenuModal> getMenuByCat(int categoryID)
        {
            List<MenuModal> menuItems = new List<MenuModal>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetMenuByCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryID", categoryID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MenuModal menuItem = new MenuModal
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                MenuName = reader["MenuName"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                               
                            };

                            menuItems.Add(menuItem);
                        }
                    }
                }
            }

            return menuItems;
        }

        public string Save(MenuModal menuModal)
        {
            
            using(SqlConnection connection= new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters parameters =new DynamicParameters();
                parameters.Add("@CategoryId",menuModal.CategoryID,DbType.Int32);
                parameters.Add("@Name",menuModal.MenuName,DbType.String);
                parameters.Add("@Description",menuModal.Description,DbType.String);
                parameters.Add("@UserId",0,DbType.Int16);
                parameters.Add("@Price", menuModal.Price);
                 parameters.Add("@ErrorMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                connection.Execute("[Insert_Update_Menu]",parameters,commandType: CommandType.StoredProcedure);
                
                return parameters.Get<string>("@ErrorMessage");
            }
            
        }

        public string SaveOrder(KitchenOrderModal order)
        {
          using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("Insert_KitchenOrderWithDetails_XML", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TableID", order.TableID);
                command.Parameters.AddWithValue("@OrderType", order.OrderType);
                command.Parameters.AddWithValue("@CreatedBy", 99);
                command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                command.Parameters.AddWithValue("@OrderId", order.OrderID);
                string detailsXml = order.OrderDetails.GenerateXml();
                command.Parameters.AddWithValue("@DetailsXML", detailsXml);
                
                    SqlParameter outputParameter = new SqlParameter("@OrderIDOut", SqlDbType.Int);
                    outputParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParameter);

                // Execute the stored procedure
                command.ExecuteNonQuery().ToString();

                return Convert.ToString(outputParameter.Value);
            }
        }
        }

         public List<MenuModal> getMenu(int pagIndex,int pageSize)
        {
            List<MenuModal> menuItems = new List<MenuModal>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[Get_ALL_Menu]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PageNumber", pagIndex);
                    command.Parameters.AddWithValue("@pageSize", pageSize);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MenuModal menuItem = new MenuModal
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                MenuName = reader["MenuName"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                CatName=reader["MenuCatName"].ToString(),                            };
                            
                            
                            menuItems.Add(menuItem);
                        }
                    }
                }
            }

            return menuItems;
        }


    
    }
}