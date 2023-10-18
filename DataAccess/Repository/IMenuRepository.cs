using System.Collections.Generic;
using RestoAppAPI.Modal;
namespace RestoAppAPI.Repository
{
    public interface IMenuRepository
    {
        List<MenuModal> getMenuByCat(int CategoryID);

        string SaveOrder(KitchenOrderModal kitchenOrderModal);

        string Save(MenuModal menuModal);
    }
}