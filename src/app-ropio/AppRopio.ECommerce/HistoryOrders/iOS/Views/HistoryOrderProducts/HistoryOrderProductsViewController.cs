using System;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.iOS.Models;
using AppRopio.ECommerce.HistoryOrders.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;
using AppRopio.ECommerce.HistoryOrders.Core;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders
{
    public partial class HistoryOrderProductsViewController : CommonViewController<IHistoryOrderProductsViewModel>
    {
		protected HistoryOrdersThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IHistoryOrdersThemeConfigService>().ThemeConfig; } }

		public HistoryOrderProductsViewController()
            : base("HistoryOrderProductsViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderProducts_Title");

            SetupTableView(TableView);
        }

        protected override void BindControls()
        {
            var bindingSet = this.CreateBindingSet<HistoryOrderProductsViewController, IHistoryOrderProductsViewModel>();

            BindTableView(TableView, bindingSet);

            bindingSet.Apply();
        }

		#endregion

		protected virtual void SetupTableView(UITableView tableView)
		{
            tableView.RegisterNibForCellReuse(HistoryOrderProductCell.Nib, HistoryOrderProductCell.Key);

            tableView.RowHeight = (nfloat)ThemeConfig.HistoryOrderItemCell.Size.Height;
			tableView.TableFooterView = new UIView();
		}

		#region BindingControls

		protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<HistoryOrderProductsViewController, IHistoryOrderProductsViewModel> set)
		{
			var dataSource = SetupTableViewDataSource(tableView);

			tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

			tableView.ReloadData();
		}

		protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
		{
            return new MvxStandardTableViewSource(tableView, HistoryOrderProductCell.Key)
			{
				DeselectAutomatically = true
			};
		}

		#endregion
	}
}