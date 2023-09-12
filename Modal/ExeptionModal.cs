namespace RestoAppAPI.Modal
{
    public class ExceptionModal
    {
        public int LineNumber { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public string StackTrace { get; set; }
        public string ErrorMessage { get; set; }
    }

}