using System.Collections.Generic;
using RestoAppAPI.Dtos;
using RestoAppAPI.Service;

namespace RestoAppAPI.Repository
{
    public class DashboardSerive : IDashboardService
    {
        private IDashBoardRepository _dashBoardRepository;
        public DashboardSerive(IDashBoardRepository dashBoardRepository)
        {
            this._dashBoardRepository=dashBoardRepository;
        }
        public List<CatWiseTotalDto> GetCatWiseTotal(string startDate, string endDate)
        {
           return this._dashBoardRepository.GetCatWiseTotal(startDate,endDate);
        }

        public List<OrderTypeWiseCountDto> GetOrderTypeWiseCount(string startDate, string endDate)
        {
            return this._dashBoardRepository.GetOrderTypeWiseCount(startDate,endDate);
        }
    }
}