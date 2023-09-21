using System.Collections.Generic;
using RestoAppAPI.Dtos;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public interface IOrderRepository
    {
        OrderViewModal GetOrder(int orderId);
         List<OrderViewModal> getALLOrders();
         List<KitchenOrderDetailViewModal> KichenOrdersByTable(string tableIds);

         string SaveTableBilling(string tableIds);

        List<KitchenOrderDto> KitchenOrdersByOrderId(int OrderId);
    }
}