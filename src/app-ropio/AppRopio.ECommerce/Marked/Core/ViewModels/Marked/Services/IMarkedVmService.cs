using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Marked.Core.ViewModels.Marked.Services
{
    public interface IMarkedVmService
    {
		Task<MvxObservableCollection<ICatalogItemVM>> LoadMarkedProducts(int count, int offset = 0);
    }
}