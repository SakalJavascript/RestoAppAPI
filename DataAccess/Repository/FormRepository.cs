using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public class FormRepository : IFormRepository
    {
        private readonly IConfiguration _configuration;

          public FormRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public List<FormMasterModal> GetAllForms()
        {
             using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();             

                return connection.Query<FormMasterModal>("Get_ALL_Forms", commandType: CommandType.StoredProcedure).AsList();
            }
        }
    }
}