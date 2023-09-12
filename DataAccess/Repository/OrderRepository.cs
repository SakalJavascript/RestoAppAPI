using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;
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
                        order.TableID=(int)row["TableID"];                      
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

         public List<OrderViewModal> getALLOrders()
        {
            List<OrderViewModal> orders = new List<OrderViewModal>();

            using (SqlConnection connection = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Get_ALL_Orders", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                   

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
    }
}