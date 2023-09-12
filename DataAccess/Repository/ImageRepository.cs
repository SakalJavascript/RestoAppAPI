using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public class ImageRepository : IImageRepository
    {
         private readonly IConfiguration _configuration;
      
        public ImageRepository(IConfiguration configuration)
        {
            this._configuration = configuration;            
        }        
 

        public ImageModal Save(ImageModal image)
        {   
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
              
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("Insert_Update_Image1", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        command.Parameters.AddWithValue("@Id", image.Id);
                        command.Parameters.AddWithValue("@FileName", image.FileName);
                        command.Parameters.AddWithValue("@FileDescription", image.FileDescription);
                        command.Parameters.AddWithValue("@FilePath", image.FilePath); // Assuming you want to use the uploaded file's name as the FilePath
                        command.Parameters.AddWithValue("@CreatedBy", 1);
                        command.Parameters.AddWithValue("@UpdatedBy", 1);

                        // Output parameter
                        SqlParameter errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 255);
                        errorMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(errorMessageParam);

                        SqlParameter insertedId = new SqlParameter("@InsertedId", SqlDbType.Int);
                        insertedId.Direction = ParameterDirection.Output;
                        command.Parameters.Add(insertedId);

                        // Execute the command
                        command.ExecuteNonQuery();
                         
                        image.Id=(int)command.Parameters["@InsertedId"].Value;
                        image.ValidationMessage=command.Parameters["@ErrorMessage"].Value.ToString();
                        
                        // Retrieve the output parameter value
                        return image;
                    }
               
            }
        }
    }
    
}