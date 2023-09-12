using System.Collections.Generic;
using RestoAppAPI.Modal;
using RestoAppAPI.Repository;

namespace RestoAppAPI.Service
{
    public class OrderSerive : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderSerive(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public OrderViewModal GetOrder(int orderId)
        {
            return this.orderRepository.GetOrder(orderId);
        }
        public  List<OrderViewModal> getALLOrders()
        {
            return this.orderRepository.getALLOrders();
        }
    }
}