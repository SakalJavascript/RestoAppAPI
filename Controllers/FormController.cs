using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Service;

namespace RestoAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    { 
       private readonly  IFrormMasterService  _service;
        public FormController(IFrormMasterService service)
        {
            this._service=service;
        }

        [HttpGet()]       
        public IActionResult Get()
        {           
            
            return Ok(this._service.GetAllForms());
        }
    }
}