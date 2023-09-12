using System.Collections.Generic;
using RestoAppAPI.Modal;

namespace RestoAppAPI.Service
{
    public interface IMenuSerive
    {
        List<MenuModal>GetMenuByCat(int categoryID);
         string SaveOrder(KitchenOrderModal kitchenOrderModal);
    }
}