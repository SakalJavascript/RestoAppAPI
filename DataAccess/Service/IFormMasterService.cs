using System.Collections.Generic;
using RestoAppAPI.Modal;
namespace RestoAppAPI.Service
{
    public interface IFrormMasterService
    {
        List<FormMasterModal> GetAllForms();
    }
}