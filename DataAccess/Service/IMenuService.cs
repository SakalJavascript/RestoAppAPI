using System.Collections.Generic;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Service
{
    public interface IMenuSerive
    {
        List<MenuModal>GetMenuByCat(int categoryID);
         string SaveOrder(KitchenOrderModal kitchenOrderModal);

         string Save(MenuModal menuModal);

          public List<MenuModal> getMenu(int pagIndex,int pageSize);
         public int getTotalMenuCount();

         public MenuModal GetById(int Id);
    }
}