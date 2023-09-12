using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Modal;
using RestoAppAPI.Service;

namespace RestoAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController:ControllerBase
    {
        private readonly IMenuSerive menuService;

        public MenuController(IMenuSerive menuService)
        {
            this.menuService = menuService;
        }

        [HttpGet()]       
        public IActionResult Get(int categoryID)
        {            
            
            return Ok(this.menuService.GetMenuByCat(categoryID));
        }

        
        [HttpPost()]       
        public IActionResult SaveOrder(KitchenOrderModal orderModal)
        {            
            
            return Ok(this.menuService.SaveOrder(orderModal));
        }


    }
}