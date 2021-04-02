using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Products.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Products.API.Services.Fakes
{
    public class CategoriesFakeService : ICategoriesService
    {
        public bool IsRussianCulture => Mvx.IoCProvider.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        #region Fields

        List<Category> _categories;

        List<Category> _subCategories;

        List<Category> _subSubcategories;

        #endregion

        public CategoriesFakeService()
        {
            _categories = new List<Category>
            {
                new Category
                {
                    Id = "-",
                    Name = IsRussianCulture ? "Горячие новинки" : "Hot",
                    //BackgroundImageUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "0",
                    Name = IsRussianCulture ? "Все товары" : "All products",
                    //BackgroundImageUrl = "https://image.shutterstock.com/z/stock-photo-canapes-sandwich-of-rye-bread-on-a-wooden-base-529082098.jpg",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "1",
                    Name = IsRussianCulture ? "Гостиная" : "Category 1",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "2",
                    Name = IsRussianCulture ? "Спальня" : "Category 2",
                    //BackgroundImageUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "3",
                    Name = IsRussianCulture ? "Мебель для кафе" : "Category 3",
                    //BackgroundImageUrl = "https://image.shutterstock.com/z/stock-photo-succulent-thick-juicy-portions-of-grilled-fillet-steak-served-with-tomatoes-and-roast-vegetables-on-138421859.jpg",
                    ContainerType = CategoryContainerType.Categories
                },
                new Category
                {
                    Id = "4",
                    Name = IsRussianCulture ? "Ванная" : "Category 4",
                    //BackgroundImageUrl = "nophoto",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "5",
                    Name = IsRussianCulture ? "Детская" : "Category 5",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "6",
                    Name = IsRussianCulture ? "Акции" : "Category 6",
                    ContainerType = CategoryContainerType.Categories
                }
            };

            _subCategories = new List<Category>
            {
                new Category
                {
                    Id = "30",
                    Name = IsRussianCulture ? "Стулья для столовой" : "Subcategory 1",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "31",
                    Name = IsRussianCulture ? "Кресла" : "Subcategory 2",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "32",
                    Name = IsRussianCulture ? "Барная мебель" : "Subcategory 3",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "33",
                    Name = IsRussianCulture ? "Столы" : "Subcategory 4",
                    ContainerType = CategoryContainerType.Categories
                },
                new Category
                {
                    Id = "34",
                    Name = IsRussianCulture ? "Комплекты для столовой" : "Subcategory 5",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "35",
                    Name = IsRussianCulture ? "Высокие стулья" : "Subcategory 6",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "36",
                    Name = IsRussianCulture ? "Офисные стулья" : "Subcategory 7",
                    ContainerType = CategoryContainerType.Products
                }
            };

            _subSubcategories = new List<Category>
            {
                new Category
                {
                    Id = "337",
                    Name = IsRussianCulture ? "Горячие новинки" : "Sub-subcategory 1",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "330",
                    Name = IsRussianCulture ? "Все товары" : "Sub-subcategory 2",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "331",
                    Name = IsRussianCulture ? "Гостиная" : "Sub-subcategory 3",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "332",
                    Name = IsRussianCulture ? "Спальня" : "Sub-subcategory 4",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "333",
                    Name = IsRussianCulture ? "Мебель для кафе" : "Sub-subcategory 5",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "334",
                    Name = IsRussianCulture ? "Ванная" : "Sub-subcategory 6",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "335",
                    Name = IsRussianCulture ? "Детская" : "Sub-subcategory 7",
                    ContainerType = CategoryContainerType.Products
                },
                new Category
                {
                    Id = "336",
                    Name = IsRussianCulture ? "Кухня" : "Sub-subcategory 8",
                    ContainerType = CategoryContainerType.Products
                }
            };
        }

        #region ICategoriesService implementation

        public async Task<List<Category>> LoadCategories(string categoryId)
        {
            await Task.Delay(500);

            if (categoryId == "6")
                return _subSubcategories;

            return !string.IsNullOrEmpty(categoryId) && (_categories.Any(x => x.Id == categoryId) || _subCategories.Any(x => x.Id == categoryId)) ?
                          (_subCategories.Any(x => x.Id == categoryId) ? _subSubcategories : _subCategories)
                              :
                          _categories;
        }

        public async Task<Category> LoadCategoryById(string categoryId)
        {
            await Task.Delay(500);

            return _categories.Union(_subCategories).FirstOrDefault(c => c.Id == categoryId);
        }

        #endregion
    }
}
