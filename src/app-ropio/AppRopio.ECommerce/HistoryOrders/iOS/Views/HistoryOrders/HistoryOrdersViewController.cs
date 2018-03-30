using System;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.iOS.Models;
using AppRopio.ECommerce.HistoryOrders.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders
{
    public partial class HistoryOrdersViewController : CommonViewController<IHistoryOrdersViewModel>
    {
        protected MvxUIRefreshControl _refreshControl;

		protected HistoryOrdersThemeConfig ThemeConfig { get { return Mvx.Resolve<IHistoryOrdersThemeConfigService>().ThemeConfig; } }

        public HistoryOrdersViewController()
            : base("HistoryOrdersViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = "История";

			_refreshControl = new MvxUIRefreshControl();

			SetupTableView(TableView);
        }

        protected override void BindControls()
        {
            var bindingSet = this.CreateBindingSet<HistoryOrdersViewController, IHistoryOrdersViewModel>();

            BindTableView(TableView, bindingSet);

			bindingSet.Apply();
        }

		#endregion

		protected virtual void SetupTableView(UITableView tableView)
		{
            tableView.RegisterNibForCellReuse(HistoryOrderCell.Nib, HistoryOrderCell.Key);

            tableView.RowHeight = (nfloat)ThemeConfig.HistoryOrderCell.Size.Height;
			tableView.TableFooterView = new UIView();

            tableView.RefreshControl = _refreshControl;
        }

		#region BindingControls

		protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<HistoryOrdersViewController, IHistoryOrdersViewModel> set)
		{
			var dataSource = SetupTableViewDataSource(tableView);

			tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Orders);
			set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

			tableView.ReloadData();

            set.Bind(_refreshControl).For(r => r.IsRefreshing).To(vm => vm.Reloading);
            set.Bind(_refreshControl).For(r => r.RefreshCommand).To(vm => vm.ReloadCommand);
		}

		protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
		{
            return new MvxStandardTableViewSource(tableView, HistoryOrderCell.Key)
			{
				DeselectAutomatically = true
			};
		}

        #endregion
    }
}

