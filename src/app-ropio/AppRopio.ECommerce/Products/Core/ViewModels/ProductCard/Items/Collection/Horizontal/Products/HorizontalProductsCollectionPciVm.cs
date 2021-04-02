using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.API.Services;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using AppRopio.Models.Products.Responses;
using MvvmCross;
using MvvmCross.Logging;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Products
{
    public class HorizontalProductsCollectionPciVm : BaseCollectionPciVm<ICatalogItemVM, Product>, IHorizontalProductsCollectionPciVm
    {
        #region Properties

        protected string GroupId { get; }

        protected string ProductId { get; }

        #endregion

        #region Services

        protected IProductService ApiService => Mvx.IoCProvider.Resolve<IProductService>();

        protected new IProductsNavigationVmService NavigationVmService => Mvx.IoCProvider.Resolve<IProductsNavigationVmService>();

        #endregion

        #region Constructor

        public HorizontalProductsCollectionPciVm(string groupId, string productId, ProductParameter parameter)
            : base(parameter)
        {
            GroupId = groupId;
            ProductId = productId;
            Items = new ObservableCollection<ICatalogItemVM>();
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected override async void LoadContent()
        {
            Loading = true;

            try
            {
                var result = await ApiService.LoadProductsCompilationForParameter(GroupId, ProductId, Id);
                var dataSource = new ObservableCollection<ICatalogItemVM>(result.Select(x => SetupItem(DataType, x)));

                InvokeOnMainThread(() => Items = dataSource);
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"{this.GetType().FullName}: {ex.BuildAllMessagesAndStackTrace()}");
            }

            Loading = false;
        }

        protected override ICatalogItemVM SetupItem(ProductDataType dataType, Product model)
		{
			return new CatalogItemVM(model);
		}

		protected override Task BuildSelectedValue(bool withNotifyPropertyChanged = true)
		{
			return Task.Delay(0);
		}

        protected override void OnItemSelected(ICatalogItemVM item)
        {
            NavigationVmService.NavigateToProduct(new Models.Bundle.ProductCardBundle(item.Model, Base.Core.Models.Navigation.NavigationType.DoublePush));
        }

        #endregion

        #region Public

        public override void ClearSelectedValue()
        {

        }

        #endregion
    }
}
