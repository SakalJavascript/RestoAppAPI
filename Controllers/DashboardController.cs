using System;
using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Service;

namespace RestoAppAPI.Controllers
{
    [Route("api/[Controller]")]
    public class DashboardController:ControllerBase
    {
        private readonly IDashboardService  _IDashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this._IDashboardService = dashboardService;
        }

        [HttpGet("{startDate}/{endDate}")]
        public IActionResult GetCatWiseTotal(string startDate, string endDate)
        {            
            return Ok( this._IDashboardService.GetCatWiseTotal(startDate.Replace("%2F", "/"),endDate.Replace("%2F", "/")));
        }

        [HttpGet("order-type-wise-count/{startDate}/{endDate}")]
        public IActionResult Get(string startDate, string endDate)
        {            
            return Ok( this._IDashboardService.GetOrderTypeWiseCount(startDate.Replace("%2F", "/"),endDate.Replace("%2F", "/")));
        }


    }
}