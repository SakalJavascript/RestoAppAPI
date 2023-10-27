using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RestoAppAPI.Dtos;
using RestoAppAPI.Helpers;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
      
        public OrderRepository(IConfiguration configuration)
        {
            this._configuration = configuration;            
        }        

        public OrderViewModal GetOrder(int orderId)
        {
            var order=new OrderViewModal();
            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Get_Orders_ById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    command.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;

                    // Create a DataSet to store the results
                    DataSet dataSet = new DataSet();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Fill the DataSet with the results of the stored procedure
                        adapter.Fill(dataSet);
                    }

                    // Access the first result set (Order information)
                    DataTable orderTable = dataSet.Tables[0];
                    
                    foreach (DataRow row in orderTable.Rows)
                    {
                        order.OrderId = (int)row["OrderId"];
                        order.TotalAmount = (decimal)row["TotalAmount"];
                        order.OrderType= (byte)row["OrderType"];
                        order.OrderDate=(DateTime)row["OrderDate"];
                        order.IsPaid=(Boolean)row["IsPaid"];
                                           
                    }
                    Console.WriteLine();

                    // Access the second result set (Menu items)
                    DataTable menuTable = dataSet.Tables[1];  
                                      
                    foreach (DataRow row in menuTable.Rows)
                    {
                        order.KitchenOrders.Add(
                            new KitchenOrderDetailViewModal{
                                KitchenOrdersId=(int)row["KitchenOrdersId"],
                                 MenuName = (string)row["MenuName"],
                                 Price = (decimal)row["Price"],
                                 Quantity = (int)row["Quantity"], 

                            }
                        );   
                       
                    }
                }
            }
            return order;
        }

        
        public int getALLOrdersCount()
        {
          
            var Total=0;
            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();                
                using (SqlCommand command = new SqlCommand("[Get_ALL_Orders_Count]", connection))
                {
              
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Total = Convert.ToInt32(reader["Total"]);                               
                         
                        }
                    }
                }
            }

            return Total;
        }
    

         public List<OrderViewModal> getALLOrders(int PageNumber, int pageSize)
        {
            List<OrderViewModal> orders = new List<OrderViewModal>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Get_ALL_Orders", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PageNumber",PageNumber);
                    command.Parameters.AddWithValue("@pageSize",pageSize);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderViewModal order = new OrderViewModal
                            {
                                OrderId = Convert.ToInt32(reader["ID"]),
                                OrderDate = (DateTime)reader["OrderDate"] ,
                                TotalAmount =(decimal) reader["TotalAmount"],
                                IsPaid = Convert.ToBoolean(reader["IsPaid"]),
                                OrderType = Convert.ToInt16(reader["OrderType"]),
                               
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }
    

         public List<KitchenOrderDetailViewModal> KichenOrdersByTable(string tableIds)
        {
            var KitchenOrders=new List<KitchenOrderDetailViewModal>();
            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[Get_Table_bills]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    string detailsXml = tableIds.GenerateXml();
                    command.Parameters.AddWithValue("@TableIds", detailsXml);

                    // Create a DataSet to store the results
                    DataSet dataSet = new DataSet();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Fill the DataSet with the results of the stored procedure
                        adapter.Fill(dataSet);
                    }  
                    DataTable menuTable = dataSet.Tables[0]; 
                                      
                    foreach (DataRow row in menuTable.Rows)
                    {
                       KitchenOrders.Add(
                            new KitchenOrderDetailViewModal{
                                KitchenOrdersId=(int)row["KitchenOrdersId"],
                                 MenuName = (string)row["MenuName"],
                                 Price = (decimal)row["Price"],
                                 Quantity = (int)row["Quantity"], 

                            }
                        );   
                       
                    }
                }
            }
            return KitchenOrders;
        }

        public string SaveTableBilling(string tableIds)
        {   
             using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Insert_Table_Billing", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandType = CommandType.StoredProcedure;
                    string detailsXml = tableIds.GenerateXml();
                    command.Parameters.AddWithValue("@TableIds", detailsXml);                    
                
                    SqlParameter outputParameter = new SqlParameter("@OrderIDOut", SqlDbType.Int);
                    outputParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParameter);

                    // Execute the stored procedure
                    command.ExecuteNonQuery().ToString();

                    return Convert.ToString(outputParameter.Value);
                }
            }
            
        }

        public List<KitchenOrderDto> KitchenOrdersByOrderId(int OrderId)
        {
           List<KitchenOrderDto> orders = new List<KitchenOrderDto>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[Get_KitchenOrders_ById]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                      command.Parameters.Add("@OrderId", SqlDbType.Int).Value = OrderId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KitchenOrderDto order = new KitchenOrderDto
                            {
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                MenuName = Convert.ToString(reader["MenuName"] )                              
                            };
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }
    }
}