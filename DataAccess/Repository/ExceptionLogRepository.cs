using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExceptionLogRepository(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            this._configuration = configuration;
            this._webHostEnvironment = webHostEnvironment;
        }        

        public void log(ExceptionModal exceptionModal)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand command = new SqlCommand("Insert_Exception_Log", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LineNumber", exceptionModal.LineNumber);
                    command.Parameters.AddWithValue("@MethodName", exceptionModal.MethodName);
                    command.Parameters.AddWithValue("@ClassName", exceptionModal.ClassName);
                    command.Parameters.AddWithValue("@StackTrace", exceptionModal.StackTrace);
                    command.Parameters.AddWithValue("@ErrorMessage", exceptionModal.ErrorMessage);
                    connection.Open();
                    command.ExecuteNonQuery();
            }

            }
            catch (System.Exception ex)
            {
               var localFilePath= Path.Combine(this._webHostEnvironment.ContentRootPath,"Logs","exception.txt");
              using(StreamWriter streamWriter=new StreamWriter(localFilePath,true))
              {
                        streamWriter.WriteLine($"-----------------------{DateTime.Now}---------------------------------");
                        streamWriter.WriteLine($"Message: {ex.Message}");
                        streamWriter.WriteLine($"Line Number: {ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(":line ") + 6)}");
                        streamWriter.WriteLine($"Stack Trace: {ex.StackTrace}");
                        streamWriter.WriteLine("--------------------------------------------------------");
              }
                
            }
        }
    }
}