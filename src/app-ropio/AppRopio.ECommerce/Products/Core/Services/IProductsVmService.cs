using System;
using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Products.Core.Services
{
    public interface IProductsVmService
    {
        Task<IMvxViewModel> LoadCartIndicatorViewModel();
    }
}
