using System.Collections.Generic;
using RestoAppAPI.Modal;
using RestoAppAPI.Repository;

namespace RestoAppAPI.Service
{
    public class FormMasterService : IFrormMasterService
    {
        private readonly IFormRepository _repository;
        public FormMasterService(IFormRepository repository)
        {
            this._repository=repository;
        }
        public List<FormMasterModal> GetAllForms()
        {
           return this._repository.GetAllForms();
        }
    }
}