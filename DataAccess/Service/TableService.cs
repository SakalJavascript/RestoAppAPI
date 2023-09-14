using System.Collections.Generic;
using RestoAppAPI.Dtos;
using RestoAppAPI.Modal;
using RestoAppAPI.Repository;

namespace RestoAppAPI.Service
{
    public class TableService : ITableService
    {
        private readonly ITableRepository tableRepository;

        public TableService(ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public void ChnangeTable(ChangeTableDtos changeTableDtos)
        {
             tableRepository.ChnangeTable(changeTableDtos);
        }

        public List<TableModal> GetAllTables()
        {
            return tableRepository.GetAllTables();
        }
    }
}