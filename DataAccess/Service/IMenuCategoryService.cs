using System.Collections.Generic;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Service
{
    public interface IMenuCategoryService
    {
         List<MenuCategoryModal> GetMenuCategory(int Pagesize=10, int PageNumber=1);
         string  SaveMenuCategory(MenuCategoryModal menuCategoryModal);

         List<MenuCategoryModal> GetBySearchText(string searchText);
        
        int GetAllCount();
         public MenuCategoryModal GetById(int Id);
    }
}