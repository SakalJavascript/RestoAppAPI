using System.Collections.Generic;
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
        public List<TableModal> GetAllTables()
        {
            return tableRepository.GetAllTables();
        }
    }
}