using System.Collections.Generic;
using RestoAppAPI.Dtos;

namespace RestoAppAPI.Service
{
    public interface IDashboardService
    {
        List<CatWiseTotalDto> GetCatWiseTotal(string startDate,string endDate);

        List<OrderTypeWiseCountDto> GetOrderTypeWiseCount(string startDate, string endDate);
    }
}