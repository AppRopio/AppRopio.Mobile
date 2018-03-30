using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services.Fakes
{
    public class CategoriesFakeService : ICategoriesService
    {
        #region Fields

        List<Category> _categories = new List<Category>
        {
            new Category
            {
                Id = "-",
                Name = "Горячие новинки",
                //BackgroundImageUrl = "https://image.shutterstock.com/z/stock-photo-tasty-macaroni-cookies-on-a-wooden-base-541322413.jpg",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "0",
                Name = "Все товары",
                //BackgroundImageUrl = "https://image.shutterstock.com/z/stock-photo-canapes-sandwich-of-rye-bread-on-a-wooden-base-529082098.jpg",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "1",
                Name = "Гостиная",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "2",
                Name = "Спальня",
                //BackgroundImageUrl = "https://image.shutterstock.com/z/stock-photo-fresh-salad-with-chicken-tomatoes-and-mixed-greens-arugula-mesclun-mache-on-wooden-background-390942682.jpg",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "3",
                Name = "Мебель для кафе",
                //BackgroundImageUrl = "https://image.shutterstock.com/z/stock-photo-succulent-thick-juicy-portions-of-grilled-fillet-steak-served-with-tomatoes-and-roast-vegetables-on-138421859.jpg",
                ContainerType = CategoryContainerType.Categories
            },
            new Category
            {
                Id = "4",
                Name = "Ванная",
                //BackgroundImageUrl = "nophoto",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "5",
                Name = "Детская",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "6",
                Name = "Акции",
                ContainerType = CategoryContainerType.Categories
            }
        };

        List<Category> _subCategories = new List<Category>
        {
            new Category
            {
                Id = "30",
                Name = "Стулья для столовой",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "31",
                Name = "Кресла",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "32",
                Name = "Барная мебель",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "33",
                Name = "Столы",
                ContainerType = CategoryContainerType.Categories
            },
            new Category
            {
                Id = "34",
                Name = "Комплекты для столовой",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "35",
                Name = "Высокие стулья",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "36",
                Name = "Офисные стулья",
                ContainerType = CategoryContainerType.Products
            }
        };

        List<Category> _subSubcategories = new List<Category>
        {
            new Category
            {
                Id = "337",
                Name = "Горячие новинки",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "330",
                Name = "Все товары",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "331",
                Name = "Гостиная",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "332",
                Name = "Спальня",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "333",
                Name = "Мебель для кафе",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "334",
                Name = "Ванная",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "335",
                Name = "Детская",
                ContainerType = CategoryContainerType.Products
            },
            new Category
            {
                Id = "336",
                Name = "Кухня",
                ContainerType = CategoryContainerType.Products
            }
        };

        #endregion

        #region ICategoriesService implementation

        public async Task<List<Category>> LoadCategories(string categoryId)
        {
            await Task.Delay(500);

            if (categoryId == "6")
                return _subSubcategories;

            return !string.IsNullOrEmpty(categoryId) && (_categories.Any(x => x.Id == categoryId) || _subCategories.Any(x => x.Id == categoryId)) ? 
                          ( _subCategories.Any(x => x.Id == categoryId) ? _subSubcategories : _subCategories) 
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
