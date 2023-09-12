using System;
namespace RestoAppAPI.Modal
{
    public class KitchenOrderDetailModal
    {
        public int ID { get; set; }
        public int KitchenOrdersId { get; set; }
        public int MenuID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }        
        public virtual KitchenOrderModal Order { get; set; }
    }
}