using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.API.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops.Items;
using AppRopio.Models.Products.Responses;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops
{
    public class HorizontalShopsCollectionPciVm : BaseCollectionPciVm<IShopAvailabilityItemVM, Shop>, IHorizontalShopsCollectionPciVm
    {
        #region Properties

        protected string GroupId { get; }

        protected string ProductId { get; }

        #endregion

        #region Services

        protected IProductService ApiService => Mvx.Resolve<IProductService>();

        #endregion

        #region Constructor

        public HorizontalShopsCollectionPciVm(string groupId, string productId, ProductParameter parameter)
            : base(parameter)
        {
            GroupId = groupId;
            ProductId = productId;
            Items = new ObservableCollection<IShopAvailabilityItemVM>();
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
                var result = await ApiService.LoadShopsCompilationForParameter(GroupId, ProductId, Id);
                var dataSource = new ObservableCollection<IShopAvailabilityItemVM>(result.Select(x => SetupItem(DataType, x)));

                InvokeOnMainThread(() => Items = dataSource);
            }
            catch (Exception ex)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, this.GetType().FullName, ex.BuildAllMessagesAndStackTrace());
            }

            Loading = false;
        }

        protected override IShopAvailabilityItemVM SetupItem(ProductDataType dataType, Shop value)
        {
            return new ShopAvailabilityItemVM(dataType, value);
        }

        protected override Task BuildSelectedValue(bool withNotifyPropertyChanged = true)
        {
            return Task.Delay(0);
        }

        protected override void OnItemSelected(IShopAvailabilityItemVM item)
        {

        }

        #endregion

        #region Public

        public override void ClearSelectedValue()
        {

        }

		public static bool operator ==(HorizontalShopsCollectionPciVm a, HorizontalShopsCollectionPciVm b)
		{
			// If both are null, or both are same instance, return true.
			if (System.Object.ReferenceEquals(a, b))
			{
				return true;
			}

			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
			{
				return false;
			}

			// Return true if the fields match:
            return a.DataType == b.DataType && a.Id == b.Id;
		}

		public static bool operator !=(HorizontalShopsCollectionPciVm a, HorizontalShopsCollectionPciVm b)
		{
			return !(a == b);
		}

        #endregion
    }
}
