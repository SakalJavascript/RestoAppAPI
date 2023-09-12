using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Modal;
using RestoAppAPI.Service;
namespace RestoAppAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {         
        private readonly IImageService imageService;
        public ImageController(IImageService imageService)
        {
            this.imageService=imageService;
        }   

        [HttpPost()]
        
        public async Task<IActionResult> Save([FromForm] ImageModal image)
        {            
            var imageModal= await this.imageService.Save(image);
            if(imageModal.ValidationMessage!=string.Empty)
            {
                return BadRequest(imageModal.ValidationMessage);

            }
            return Ok();
            
        }
    }
}
