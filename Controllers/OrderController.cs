using Microsoft.AspNetCore.Mvc;
using RestoAppAPI.Service;

namespace RestoAppAPI.Controllers
{   
    [Route("api/[controller]")]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("{orderId}")]
        public IActionResult Get(int orderId)
        {
            return Ok(this.orderService.GetOrder(orderId));
        }

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(this.orderService.getALLOrders());
        }
    }
}