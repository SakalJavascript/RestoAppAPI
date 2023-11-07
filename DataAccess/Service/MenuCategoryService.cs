using System.Collections.Generic;
using System.Threading.Tasks;
using RestoAppAPI.Modal;
using RestoAppAPI.Repository;
namespace RestoAppAPI.Service
{
    public class MenuCategoryService : IMenuCategoryService
    {
       
        private readonly IMenuCategoryRepository _categoryRepository;
        private readonly IImageService _imageService;

        public MenuCategoryService(IMenuCategoryRepository categoryRepository,
                                   IImageService imageService)
        {
            _categoryRepository=categoryRepository;
            _imageService=imageService;
        }
        public List<MenuCategoryModal> GetMenuCategory(int Pagesize=10, int PageNumber=1)
        {
           return  _categoryRepository.GetByPagination( Pagesize, PageNumber);
        }

        public string SaveMenuCategory(MenuCategoryModal menuCategory)
        {               
              return _categoryRepository.Save(menuCategory);               
        }       

        public List<MenuCategoryModal> GetBySearchText(string searchText)
        {
           return _categoryRepository.GetBySearchText(searchText);
        }

        public int GetAllCount()
        {
            return _categoryRepository.GetAllCount();
        }

        public MenuCategoryModal GetById(int Id)
        {
            return _categoryRepository.GetById(Id);
        }
    }
}