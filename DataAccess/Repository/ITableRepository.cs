using System.Collections.Generic;
using RestoAppAPI.Dtos;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public interface ITableRepository
    {
        List<TableModal> GetAllTables();
        void ChnangeTable(ChangeTableDtos changeTableDtos);
    }
}