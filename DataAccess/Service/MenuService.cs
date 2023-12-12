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

        public MenuModal GetById(int Id)
        {
            return this.menuRepository.GetById(Id);
        }

        public List<MenuModal> getMenu(int pagIndex, int pageSize)
        {
            return this.menuRepository.getMenu(pagIndex,pageSize);
        }

        public List<MenuModal> GetMenuByCat(int categoryID)
        {
            return this.menuRepository.getMenuByCat(categoryID);
        }

        public int getTotalMenuCount()
        {
           return this.menuRepository.getTotalMenuCount();
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