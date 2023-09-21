using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Service;

namespace RestoAppAPI.Controllers
{   
    [Route("api/[controller]")]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrderController(IOrderService orderService,
                                IWebHostEnvironment webHostEnvironment
        )
        {
            this.orderService = orderService;
            this._webHostEnvironment=webHostEnvironment;
        }

        [HttpGet("{orderId:int}")]
        public IActionResult Get(int orderId)
        {
            return Ok(this.orderService.GetOrder(orderId));
        }

        [HttpGet("kitchen/{orderId:int}")]
        public IActionResult kitchen(int orderId)
        {
            return Ok(this.orderService.KitchenOrdersByOrderId(orderId));
        }
        
        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(this.orderService.getALLOrders());
        }

        [HttpGet("table-bills/{TableIds}")]
        public IActionResult Get(string TableIds)
        {                  
            return Ok(this.orderService.KichenOrdersByTable(TableIds));
        }
        [HttpPost("table-bills/{TableIds}")]
        public IActionResult post(string TableIds)
        {                  
            return Ok(this.orderService.SaveTableBilling(TableIds));
        }

        [HttpGet("pdf")]
        public IActionResult GetPdf()
        {
            var directory = _webHostEnvironment.ContentRootPath; 
            var filePath = Path.Combine(directory, "Pdfs", "test.pdf");
           
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // PDF file not found
            }

            var fileStream = System.IO.File.OpenRead(filePath);
            return File(fileStream, "application/pdf");
        }

    }
}