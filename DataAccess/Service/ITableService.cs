using System.Collections.Generic;
using RestoAppAPI.Dtos;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Service
{
    public interface ITableService
    {
        List<TableModal> GetAllTables();
        void ChnangeTable(ChangeTableDtos changeTableDtos);
    }
}