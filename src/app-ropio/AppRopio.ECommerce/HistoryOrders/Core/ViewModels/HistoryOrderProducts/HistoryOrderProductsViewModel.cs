using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.HistoryOrders.Core.Models.Bundle;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrderProducts.Items;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
    public class HistoryOrderProductsViewModel : BaseViewModel, IHistoryOrderProductsViewModel
    {
        #region Commands

        private IMvxCommand _selectionChangedCommand;
        public IMvxCommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IHistoryOrderProductItemVM>(OnItemSelected));
            }
        }

        #endregion

        public string OrderId { get; private set; }

        private MvxObservableCollection<IHistoryOrderProductItemVM> _items;
        public MvxObservableCollection<IHistoryOrderProductItemVM> Items
        {
            get { return _items; }
            protected set
            {
                SetProperty(ref _items, value);
            }
        }

		#region Services

        protected IHistoryOrderDetailsVmService VmService { get { return Mvx.Resolve<IHistoryOrderDetailsVmService>(); } }

		#endregion

		#region Init

		public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

			var navigationBundle = parameters.ReadAs<HistoryOrderBundle>();
			this.InitFromBundle(navigationBundle);
		}

		protected virtual void InitFromBundle(HistoryOrderBundle parameters)
		{
            OrderId = parameters.OrderId;
			VmNavigationType = parameters.NavigationType == NavigationType.None ?
															NavigationType.ClearAndPush :
															parameters.NavigationType;
		}

		#endregion

		public override Task Initialize()
		{
            return LoadContent();
		}

		//загрузка истории
		protected virtual async Task LoadContent()
		{
			Loading = true;

            var dataSource = await VmService.LoadOrderProducts(OrderId);

            InvokeOnMainThread(() => Items = dataSource);

			Loading = false;
		}

        protected virtual void OnItemSelected(IHistoryOrderProductItemVM product)
		{
            VmService.NavigateToProduct(product.Product);
		}
	}
}