using System.Collections.Generic;
using RestoAppAPI.Modal;
using RestoAppAPI.Repository;

namespace RestoAppAPI.Service
{
    public class MenuService : IMenuSerive
    {
        private readonly IMenuRepository menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }
        public List<MenuModal> GetMenuByCat(int categoryID)
        {
            return this.menuRepository.getMenuByCat(categoryID);
        }

        public string Save(MenuModal menuModal)
        {
            return this.menuRepository.Save(menuModal);
        }

        public string SaveOrder(KitchenOrderModal kitchenOrderModal)
        {
           return this.menuRepository.SaveOrder(kitchenOrderModal);
        }
    }
}