using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    }
}