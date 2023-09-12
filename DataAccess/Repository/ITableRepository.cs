using System.Collections.Generic;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public interface ITableRepository
    {
        List<TableModal> GetAllTables();
        
    }
}