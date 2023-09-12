using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public interface IExceptionLogRepository
    {
        public void log(ExceptionModal exceptionModal);
    }
}