using System.Collections.Generic;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Service
{
    public interface IOrderService
    {
        OrderViewModal GetOrder(int orderId);

         List<OrderViewModal> getALLOrders();
    }
}