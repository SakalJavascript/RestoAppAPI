using System;

namespace RestoAppAPI.Modal
{
    public class FormMasterModal
    {
        public int FormID { get; set; }
        public int? ParentFormID { get; set; }
        public string FormName { get; set; }
        public string FormURL { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}