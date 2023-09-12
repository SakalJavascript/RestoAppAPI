namespace RestoAppAPI.Modal
{
    public class TableModal
    {
        public int ID { get; set; }
        public int TableNumber { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public bool IsOccupied { get; set; }
        public double TotalAmount { get; set; }
        public int CurrentOrderId { get; set; }
    
    }

}