using System;
using System.Collections.Generic;
namespace RestoAppAPI.Modal
{
    public class KitchenOrderModal
    {
        public int ID { get; set; }
        public int TableID { get; set; }
        public int? OrderID { get; set; }  
        public float TotalAmount { get; set; }
        public int OrderType { get; set; }
        public virtual List<KitchenOrderDetailModal> OrderDetails { get; set; }
    }
}
