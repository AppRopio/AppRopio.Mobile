using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Marked.Core.ViewModels.Marked
{
    public interface IMarkedViewModel : ICatalogViewModel
	{
        bool LoadingMore { get; }
	}
}