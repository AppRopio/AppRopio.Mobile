using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Partial
{
    public partial class DeliveryTypesViewController : CommonViewController<IDeliveryViewModel>
    {
        protected Models.Order OrderTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order; } }

        public DeliveryTypesViewController()
            : base("DeliveryTypesViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<DeliveryTypesViewController, IDeliveryViewModel>();

            BindTableView(_tableView, set);

            set.Apply();
        }

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "DeliveryTypes_Title");

            SetupTableView(_tableView);
        }

        #endregion

        #region InitializationControls

        protected virtual void SetupTableView(UITableView tableView)
        {
            tableView.TableHeaderView = new UIView();
            tableView.TableFooterView = new UIView();
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            tableView.RowHeight = OrderTheme.DeliveryInfo.Cell.Size.Height ?? 50;
            tableView.SeparatorColor = (UIColor)Theme.ColorPalette.Separator;
        }

        #endregion

        #region BindingControls

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new MvxSimpleTableViewSource(tableView, DeliveryTypeCell.Key, DeliveryTypeCell.Key);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<DeliveryTypesViewController, IDeliveryViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            set.Bind(dataSource).For(s => s.ItemsSource).To(vm => vm.Items);
            set.Bind(dataSource).For(s => s.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
            set.Bind(tableView).For("AnimatedVisibility").To(vm => vm.Loading).WithConversion("InvertedVisibility");

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        #endregion
    }
}

