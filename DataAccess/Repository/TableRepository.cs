using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public class TableRepository : ITableRepository
    {
        private IConfiguration _configuration;
       public TableRepository(IConfiguration config)
       {
           this._configuration=config;
       }
        public List<TableModal> GetAllTables()
        {
              List<TableModal> tables = new List<TableModal>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Get_ALL_TABLES", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
               
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TableModal table = new TableModal
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                TableNumber = Convert.ToInt32(reader["TableNumber"]),
                                Description = reader["Description"].ToString(),
                                IsOccupied = Convert.ToBoolean(reader["IsOccupied"]),
                                Capacity=Convert.ToInt32(reader["Capacity"]),
                                TotalAmount=Convert.ToDouble(reader["TotalAmount"]),
                                CurrentOrderId=Convert.ToInt32(reader["CurrentOrderId"])
                               
                            };

                            tables.Add(table);
                        }
                    }
                }
            }

            return tables;
        }
    }
}