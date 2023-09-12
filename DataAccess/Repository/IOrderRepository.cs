using System.Collections.Generic;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public interface IOrderRepository
    {
        OrderViewModal GetOrder(int orderId);
         List<OrderViewModal> getALLOrders();
    }
}