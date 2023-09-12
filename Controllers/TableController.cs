using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Service;

namespace RestoAppAPI.Controllers
{
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
    }
}