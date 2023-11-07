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

        [HttpGet("{pageIndex}/{pageSize}")]       
        public IActionResult Get(int pageIndex, int pageSize)
        {           
            
            return Ok(this.menuService.getMenu(pageIndex,pageSize));
        }

        
        [HttpPost("SaveOrder")]       
        public IActionResult SaveOrder(KitchenOrderModal orderModal)
        {            
            
            return Ok(this.menuService.SaveOrder(orderModal));
        }

        [HttpPost()]
        public IActionResult Save(MenuModal menuModal)
        {
            return Ok(this.menuService.Save(menuModal));
        }

    }
}