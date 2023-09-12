using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RestoAppAPI.Modal;
using RestoAppAPI.Repository;

namespace RestoAppAPI.Service
{
    public class ImageService : IImageService
    {
         private readonly IWebHostEnvironment webHostEnvironment;
         private readonly IHttpContextAccessor httpContextAccessor;
         private readonly IImageRepository imageRepository;

         public ImageService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,IImageRepository imageRepository)
         {
              this.webHostEnvironment=webHostEnvironment;
              this.httpContextAccessor=httpContextAccessor;
              this.imageRepository=imageRepository;
         }
        public async Task<ImageModal> Save(ImageModal image)
        {
            var validationmessage=ValidateFileUpload(image);
            image.ValidationMessage=validationmessage;
             if (validationmessage==string.Empty)
            {            
                 
                var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", 
                $"{image.FileName}{Path.GetExtension(image.File.FileName)}");

                // Upload Image to Local Path
                using var stream = new FileStream(localFilePath, FileMode.Create);
                await image.File.CopyToAsync(stream);

                // https://localhost:1234/images/image.jpg

                var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{Path.GetExtension(image.File.FileName)}";

                image.FilePath = urlFilePath;
                // User repository to upload image
                return   imageRepository.Save(image);                

            }
            return  image;
        }
        
        private string ValidateFileUpload(ImageModal image)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(image.File.FileName)))
            {
                return  "Unsupported file extension";
            }

            if (image.File.Length > 10485760)
            {
                return  "File size more than 10MB, please upload a smaller size file.";
            }
            return string.Empty;
        }
    }
}