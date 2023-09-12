using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Dtos;
using RestoAppAPI.Modal;
using RestoAppAPI.Service;
namespace RestoAppAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MenuCategoryController : ControllerBase
    {         
        private readonly IMenuCategoryService _menuCategoryService;
      
        public MenuCategoryController(IMenuCategoryService menuCategoryService)
        {
            _menuCategoryService=menuCategoryService;
            
        }   

        [HttpGet()]
        // [Authorize]
        public IActionResult Get(int Pagesize, int PageNumber)
        {            
            var response= new ResponseDto<List<MenuCategoryModal>>();
           var menuCatList= _menuCategoryService.GetMenuCategory(Pagesize, PageNumber);
           if(menuCatList.Count>0)
           {
                response.IsSuccess=true;
                response.Data=menuCatList;            
                return Ok(response);
           }
           else
           {
                response.IsSuccess=false;
                response.Errors.Add((int)ValidationType.DataNotFount,"There are no Menu Categories");
                return NotFound(response);
           }
            
          
        }

        [HttpGet("{SearchText:alpha}")]
        public IActionResult Get(string SearchText)
        { 
            var response= new ResponseDto<List<MenuCategoryModal>>();    
            var menuCatList= _menuCategoryService.GetBySearchText(SearchText);      
            if(menuCatList.Count>0)
           {
                response.IsSuccess=true;
                response.Data=menuCatList;            
                return Ok(response);
           }
           else
           {
                response.IsSuccess=false;
                response.Errors.Add(((int)ValidationType.DataNotFount),"There are no Menu Categories");
                return NotFound(response);
           }
            
        }

        [HttpPost()]
        public IActionResult Post(MenuCategoryModal category)
        {      
            var response= new ResponseDto<MenuCategoryModal>(); 
            if(ModelState.IsValid)
            {
                string error= _menuCategoryService.SaveMenuCategory(category); 
                if(error!=string.Empty)
                {
                   return StatusCode(500, error);
                }   
                response.IsSuccess=true;               
                return Ok(response); 
            }
            else
            {
                response.IsSuccess=false;
                var  Errors = ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage).ToList();                
                response.Errors.Add((int)ValidationType.ModalValidation,string.Join(",",Errors));
                return BadRequest();
            }    
                                
        }

    }
}
