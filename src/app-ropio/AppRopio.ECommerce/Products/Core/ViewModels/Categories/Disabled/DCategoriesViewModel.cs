using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.Base.Core.Attributes;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items.Banners;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services.Banners;
using MvvmCross;
using MvvmCross.Commands;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Disabled
{
    [Deeplink("products")]
    public class DCategoriesViewModel : CatalogViewModel, IDCategoriesViewModel
    {
        #region Commands

        private IMvxCommand _bannerSelectionChangedCommand;
        public IMvxCommand BannerSelectionChangedCommand
        {
            get
            {
                return _bannerSelectionChangedCommand ?? (_bannerSelectionChangedCommand = new MvxCommand<IBannerItemVM>(OnBannerItemSelected));
            }
        }

        #endregion

        #region Properties

        private ObservableCollection<IBannerItemVM> _topBanners;
        public ObservableCollection<IBannerItemVM> TopBanners
        {
            get
            {
                return _topBanners;
            }
            set
            {
                _topBanners = value;
                RaisePropertyChanged(() => TopBanners);
            }
        }

        private ObservableCollection<IBannerItemVM> _bottomBanners;
        public ObservableCollection<IBannerItemVM> BottomBanners
        {
            get
            {
                return _bottomBanners;
            }
            set
            {
                _bottomBanners = value;
                RaisePropertyChanged(() => BottomBanners);
            }
        }

        public override bool SearchBar => ConfigService.Config.SearchType == SearchType.Bar
                                          || ConfigService.Config.SearchType == SearchType.BarOnFirstScreen;

        #endregion

        #region Services

        private IBannersVmService _bannersVmService;
        public IBannersVmService BannersVmService => _bannersVmService ?? (_bannersVmService = Mvx.IoCProvider.Resolve<IBannersVmService>());

        #endregion

        #region Constructor

        public DCategoriesViewModel()
        {
            TopBanners = new ObservableCollection<IBannerItemVM>();
            BottomBanners = new ObservableCollection<IBannerItemVM>();
        }

        #endregion

        #region Private

        private void OnBannerItemSelected(IBannerItemVM item)
        {
            NavigationVmService.NavigateTo(item.Deeplink);
        }

        #endregion

        #region Protected

        #region Init

        protected override void InitFromBundle(Models.Bundle.ProductsBundle parameters)
        {
            base.InitFromBundle(parameters);

            VmNavigationType = parameters.NavigationType == Base.Core.Models.Navigation.NavigationType.None ? Base.Core.Models.Navigation.NavigationType.ClearAndPush : parameters.NavigationType;
        }

        #endregion

        protected override async Task LoadItemsOnStart()
        {
            Loading = true;

            TopBanners = await BannersVmService.LoadTopBannersFor();
            BottomBanners = await BannersVmService.LoadBottomBannersFor();

            await LoadItems();
        }

        #endregion
    }
}
