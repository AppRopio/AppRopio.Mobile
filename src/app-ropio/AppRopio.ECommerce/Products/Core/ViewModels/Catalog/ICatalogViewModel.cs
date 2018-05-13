using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch;
using MvvmCross.Core.ViewModels;
using AppRopio.Base.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog
{
    public interface ICatalogViewModel : IProductsViewModel, IContentSearchInternalViewModel, IRefreshViewModel
    {
        bool Empty { get; }

        IMvxViewModel HeaderVm { get; }

        MvxObservableCollection<ICatalogItemVM> Items { get; }

        IMvxCommand CatalogCommand { get; }

        ICommand ShowSortCommand { get; }

        ICommand ShowFiltersCommand { get; }

        string CatalogTitle { get; }

        string NoResultsTitle { get; }

        string NoResultsText { get; }
    }
}

