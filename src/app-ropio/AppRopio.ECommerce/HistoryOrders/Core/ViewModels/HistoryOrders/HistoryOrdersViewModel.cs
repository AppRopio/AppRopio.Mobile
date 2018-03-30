using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
    public class HistoryOrdersViewModel : RefreshViewModel, IHistoryOrdersViewModel
    {
        protected int LOADING_ORDERS_COUNT = 10;

		#region Commands

		private IMvxCommand _selectionChangedCommand;

		/// <summary>
		/// Вызывается при тапе на заказ
		/// </summary>
		public IMvxCommand SelectionChangedCommand
		{
			get
			{
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IHistoryOrderItemVM>(OnOrderSelected));
			}
		}

        #endregion

        protected MvxObservableCollection<IHistoryOrderItemVM> _orders;

        public MvxObservableCollection<IHistoryOrderItemVM> Orders
        {
            get { return _orders; }
            protected set 
            {
                SetProperty(ref _orders, value);
            }
        }

		#region Services

		protected IHistoryOrdersVmService VmService { get { return Mvx.Resolve<IHistoryOrdersVmService>(); } }

        #endregion

        #region Init

        public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

			var navigationBundle = parameters.ReadAs<BaseBundle>();
			this.InitFromBundle(navigationBundle);
		}

		protected virtual void InitFromBundle(BaseBundle parameters)
		{
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

            await LoadOrders();

			Loading = false;
		}

        protected override async Task LoadMoreContent()
        {
			if (Loading || LoadingMore)
				return;

			LoadingMore = true;

			var dataSource = await VmService.LoadHistoryOrders(LOADING_ORDERS_COUNT, offset: Orders.Count);

			CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_ORDERS_COUNT;
			LoadMoreCommand.RaiseCanExecuteChanged();

			InvokeOnMainThread(() =>
			{
				Orders.AddRange(dataSource);
			});

			LoadingMore = false;
        }

        protected override Task ReloadContent()
        {
            return LoadOrders();
		}

        protected virtual async Task LoadOrders()
        {
			var dataSource = await VmService.LoadHistoryOrders(LOADING_ORDERS_COUNT);

			InvokeOnMainThread(() => Orders = dataSource);

			CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_ORDERS_COUNT;
		}

        protected virtual void OnOrderSelected(IHistoryOrderItemVM order)
		{
			VmService.HandleOrderSelection(order);
		}
	}
}
