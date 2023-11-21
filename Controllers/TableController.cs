using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Dtos;
using RestoAppAPI.Service;

namespace RestoAppAPI.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]    
    public class TableController:ControllerBase
    {
        private readonly ITableService tableService;

        public TableController(ITableService tableService)
        {
            this.tableService = tableService;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok( this.tableService.GetAllTables());
        }
        [HttpPost()]
        public IActionResult ChangeTable( [FromBody] ChangeTableDtos changeTableDtos)
        {
            this.tableService.ChnangeTable(changeTableDtos);
            return Ok();
        }
    }
}