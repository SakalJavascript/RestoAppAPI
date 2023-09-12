using System.Collections.Generic;

namespace RestoAppAPI.Dtos
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; }        
        public T Data { get; set; }
        public Dictionary<int, string> Errors { get; set; }=new Dictionary<int, string>();
    }
     
    public enum  ValidationType
    {
        DataNotFount=1,
        ModalValidation=2,
        Exception=3


    } 
}