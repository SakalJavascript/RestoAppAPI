using System.Collections.Generic;
using RestoAppAPI.Dtos;
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

        public List<KitchenOrderDetailViewModal> KichenOrdersByTable(string tableIds)
        {
             return this.orderRepository.KichenOrdersByTable(tableIds);
        }
        public string SaveTableBilling(string tableIds)
        {
            return this.orderRepository.SaveTableBilling(tableIds);
        }

        public List<KitchenOrderDto> KitchenOrdersByOrderId(int OrderId)
        {
           return this.orderRepository.KitchenOrdersByOrderId(OrderId);
        }
    }
}