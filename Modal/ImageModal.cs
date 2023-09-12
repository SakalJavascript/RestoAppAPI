using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RestoAppAPI.Modal
{
    public class ImageModal
    {
        public int? Id { get; set; }
         [Required]
        public IFormFile File { get; set; }

        
        public string FileName { get; set; }
        
        public string FileDescription { get; set; }

        public string FilePath { get; set; }
        public string ValidationMessage { get; set; }

        
    }
}