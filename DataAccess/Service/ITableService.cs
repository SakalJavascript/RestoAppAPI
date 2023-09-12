using System.Collections.Generic;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Service
{
    public interface ITableService
    {
        List<TableModal> GetAllTables();
    }
}