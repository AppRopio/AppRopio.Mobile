using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services
{
    public interface ICategoriesService
    {
        Task<List<Category>> LoadCategories(string categoryId = null);

        Task<Category> LoadCategoryById(string categoryId);
    }
}
