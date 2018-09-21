using System.Windows.Input;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels
{
    public abstract class ProductsViewModel : BaseViewModel, IProductsViewModel
    {
        #region Commands

        private ICommand _searchCommand;
        public ICommand ShowSearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new MvxCommand(OnSearchExecute));
            }
        }

        public abstract ICommand SelectionChangedCommand { get; }

        #endregion

        #region Properties

        public virtual bool SearchEnabled { get { return true; } }

        public virtual bool SearchBar { get { return ConfigService.Config.SearchType == SearchType.Bar; } }

        private IMvxViewModel _cartIndicatorVM;
        public IMvxViewModel CartIndicatorVM
        {
            get => _cartIndicatorVM;
            set => SetProperty(ref _cartIndicatorVM, value, nameof(CartIndicatorVM));
        }

        #endregion

        #region Services

        protected IProductsNavigationVmService NavigationVmService { get { return Mvx.Resolve<IProductsNavigationVmService>(); } }

        protected IProductConfigService ConfigService { get { return Mvx.Resolve<IProductConfigService>(); } }

        #endregion

        #region Protected

        protected virtual void OnSearchExecute()
        {
            NavigationVmService.NavigateToContentSearch(new BaseBundle(Base.Core.Models.Navigation.NavigationType.Push));
        }

        #endregion
    }
}
