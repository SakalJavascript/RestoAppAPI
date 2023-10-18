using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RestoAppAPI.Dtos;

namespace RestoAppAPI.Repository
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private IConfiguration _configuration;
        public DashBoardRepository(IConfiguration configuration)
        {
            this._configuration=configuration;
        }
        public List<CatWiseTotalDto> GetCatWiseTotal(string startDate, string endDate)
        {
           List<CatWiseTotalDto> list = new List<CatWiseTotalDto>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[Get_Dash_Menu_Cat_wise_total]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CatWiseTotalDto item = new CatWiseTotalDto
                            {
                                MenuCatName = Convert.ToString(reader["Name"]),
                                Total = Convert.ToDouble(reader["Total"] )                              
                            };
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

         public List<OrderTypeWiseCountDto> GetOrderTypeWiseCount(string startDate, string endDate)
        {
           List<OrderTypeWiseCountDto> list = new List<OrderTypeWiseCountDto>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[Get_Dash_Ordar_type_wise_count]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderTypeWiseCountDto item = new OrderTypeWiseCountDto
                            {
                                OrderType = Convert.ToString(reader["OrderType"]),
                                Count = Convert.ToInt32(reader["Count"] )                              
                            };
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }


        
    }
}