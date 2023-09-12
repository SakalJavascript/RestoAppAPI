using System;
using System.Collections.Generic;

namespace RestoAppAPI.Modal
{
    public class OrderViewModal
    {
        public int OrderId { get; set; }
        public int? TableID { get; set; }
        public int OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool? IsPaid { get; set; }
        public List<KitchenOrderDetailViewModal> KitchenOrders { get; set; }=new List<KitchenOrderDetailViewModal>();
    }

     public class KitchenOrderDetailViewModal
    {
        
        public int KitchenOrdersId { get; set; }
        public String MenuName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  
        public decimal Total { get{ return this.Price*this.Quantity;}  }  


       
    }

   
}