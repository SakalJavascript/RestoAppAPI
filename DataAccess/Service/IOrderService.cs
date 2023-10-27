using System.Collections.Generic;
using RestoAppAPI.Dtos;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Service
{
    public interface IOrderService
    {
        OrderViewModal GetOrder(int orderId);

         List<OrderViewModal> getALLOrders(int PageNumber, int pageSize);

          List<KitchenOrderDetailViewModal> KichenOrdersByTable(string tableIds);
          string SaveTableBilling(string tableIds);
          List<KitchenOrderDto> KitchenOrdersByOrderId(int OrderId);
          int getALLOrdersCount();
     
    }
}