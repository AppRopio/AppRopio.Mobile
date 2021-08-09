using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Partial
{
    public partial class UserViewController : CommonViewController<IUserViewModel>
    {
        protected Models.Order OrderTheme { get { return Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order; } }
        protected Models.OrderFieldCell CellTheme { get { return OrderTheme.UserInfo.Cell ?? Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.BaseOrderFieldCell; } }

        public UserViewController()
            : base("UserViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "User_Title");

            RegisterKeyboardActions = true;

            SetupTableView(_tableView);
            SetupNextButton(_nextButton);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<UserViewController, IUserViewModel>();

            BindTableView(_tableView, set);
            BindNextButton(_nextButton, set);

            set.Apply();
        }

        #endregion

        #region InitializationControls

        protected virtual void SetupTableView(UITableView tableView)
        {
            tableView.TableHeaderView = new UIView();
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            tableView.RowHeight = CellTheme.Size.Height ?? 50;
            tableView.SeparatorColor = (UIColor)Theme.ColorPalette.Separator;
        }

        protected virtual void SetupNextButton(UIButton nextButton)
        {
            nextButton.SetTitle(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "User_Continue"), UIControlState.Normal);
            nextButton.SetupStyle(OrderTheme.NextButton);
        }

        #endregion

        #region BindingControls

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new MvxSimpleTableViewSource(tableView, OrderFieldCell.Key, OrderFieldCell.Key);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<UserViewController, IUserViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            //set.Bind(dataSource).For(s => s.ItemsSource).To(vm => vm.Items);
            set.Bind(dataSource).For(s => s.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            set.Bind(tableView).For("AnimatedVisibility").To(vm => vm.Loading).WithConversion("InvertedVisibility");

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual void BindNextButton(UIButton nextButton, MvxFluentBindingDescriptionSet<UserViewController, IUserViewModel> set)
        {
            set.Bind(nextButton).To(vm => vm.NextCommand);
            set.Bind(nextButton).For(s => s.Enabled).To(vm => vm.CanGoNext);
        }

        #endregion
    }
}

