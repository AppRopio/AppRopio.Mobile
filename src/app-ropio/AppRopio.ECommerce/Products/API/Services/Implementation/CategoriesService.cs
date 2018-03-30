using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Products.Responses;
using AppRopio.Models.Products.Requests;

namespace AppRopio.ECommerce.Products.API.Services.Implementation
{
    public class CategoriesService : BaseService, ICategoriesService
    {
        protected string CATEGORIES_URL = "categories";
        protected string CATEGORY_URL = "category";

        public async Task<List<Category>> LoadCategories(string categoryId = null)
        {
            return await Post<List<Category>>(CATEGORIES_URL, ToStringContent(new CategoriesRequest { Id = categoryId }));
        }

        public async Task<Category> LoadCategoryById(string categoryId)
        {
            return await Get<Category>($"{CATEGORY_URL}/{categoryId}");
        }
    }
}
