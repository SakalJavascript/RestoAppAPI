using System.Collections.Generic;
using RestoAppAPI.Dtos;

namespace RestoAppAPI.Repository
{
    public interface IDashBoardRepository
    {
        List<CatWiseTotalDto> GetCatWiseTotal(string startDate, string endDate);

        List<OrderTypeWiseCountDto> GetOrderTypeWiseCount(string startDate, string endDate);
    }
}