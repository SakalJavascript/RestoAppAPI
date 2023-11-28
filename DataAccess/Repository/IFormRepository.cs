using System.Collections.Generic;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Repository
{
    public interface IFormRepository
    {
        List<FormMasterModal> GetAllForms();
       
    }
}