using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Navigation.Menu.Core.Models;
using AppRopio.Navigation.Menu.Core.Services;
using AppRopio.Navigation.Menu.Core.ViewModels;
using AppRopio.Navigation.Menu.iOS.Models;
using AppRopio.Navigation.Menu.iOS.Navigation;
using AppRopio.Navigation.Menu.iOS.Services;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using UIKit;
using MvvmCross.Core.Navigation;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.Navigation.Menu.iOS.Views
{
    public class MenuViewController : CommonViewController<IMenuViewModel>
    {
        protected UITableView _menuTableView;

        protected MenuNavigationPresenter NavigationPresenter { get { return Mvx.Resolve<IMvxIosViewPresenter>() as MenuNavigationPresenter; } }

        protected MenuThemeConfig ThemeConfig { get { return Mvx.Resolve<IMenuThemeConfigService>().ThemeConfig; } }

        #region Protected

        protected void ResolveTableViewHeader(MenuConfig config, UITableView tableView)
        {
            if (config.Header != null && Mvx.Resolve<IViewLookupService>().IsRegistered(config.Header.TypeName))
            {
                var headerView = ViewModel.HeaderVm == null ? null : Mvx.Resolve<IMvxIosViewCreator>().CreateView(ViewModel.HeaderVm) as UIView;
                if (headerView != null)
                    tableView.TableHeaderView = headerView;
            }

            if (tableView.TableHeaderView == null && ThemeConfig.LeftViewController.LogotypeHeaderImage != null)
                tableView.TableHeaderView = ConstructLogotypeHeader(tableView);
        }

        protected virtual UIView ConstructLogotypeHeader(UITableView tableView)
        {
            var logotypeHeaderHeight = 140f;

            var logo = new UIImageView(UIImage.FromFile(ThemeConfig.LeftViewController.LogotypeHeaderImage.Path));
            logo.Frame = new CGRect(11, logotypeHeaderHeight - logo.Image.Size.Height - 25, logo.Image.Size.Width, logo.Image.Size.Height);

            return new UIView()
                .WithFrame(0, 0, tableView.Frame.Width, logotypeHeaderHeight)
                .WithSubviews(
                    logo
                );
        }

        protected void ResolveTableViewFooter(MenuConfig config, UITableView tableView)
        {
            if (config.Footer != null && Mvx.Resolve<IViewLookupService>().IsRegistered(config.Footer.TypeName))
            {
                var footerView = ViewModel.FooterVm == null ? null : Mvx.Resolve<IMvxIosViewCreator>().CreateView(ViewModel.FooterVm) as UIView;
                if (footerView != null)
                    tableView.TableFooterView = footerView;
            }
        }

        #region InitializationControls

        protected virtual void SetupTableView(UITableView tableView)
        {
            var menuMargins = ThemeConfig.LeftViewController.MenuTable.Margins;
            var menuFrame = new CGRect(
                menuMargins.Left,
                menuMargins.Top,
                (ThemeConfig.LeftViewController.Size.Width.HasValue ? ThemeConfig.LeftViewController.Size.Width.Value : UIScreen.MainScreen.Bounds.Width) - menuMargins.Left - menuMargins.Right,
                (ThemeConfig.LeftViewController.Size.Height.HasValue ? ThemeConfig.LeftViewController.Size.Height.Value : UIScreen.MainScreen.Bounds.Height) - menuMargins.Top - menuMargins.Bottom
            );

            tableView.Frame = menuFrame;

            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.BackgroundColor = (UIColor)ThemeConfig.LeftViewController.MenuTable.Background;
            tableView.RowHeight = ThemeConfig.LeftViewController.MenuTable.MenuCell.Size.Height.Value;
        }

        protected virtual void SetupTableViewHeaderAndFooter(UITableView tableView)
        {
            var config = Mvx.Resolve<IMenuConfigService>().Config;

            ResolveTableViewHeader(config, tableView);

            ResolveTableViewFooter(config, tableView);
        }

        #endregion

        #region BindingControls

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<MenuViewController, MenuViewModel> set)
        {
            SetupTableViewHeaderAndFooter(tableView);

            var source = SetupTableViewSource(tableView);

            set.Bind(source).To(vm => vm.Items);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.Source = source;
            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewSource(UITableView tableView)
        {
            return new MenuTableViewSource(tableView, MenuCell.Key, MenuCell.Key);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            View.BackgroundColor = (UIColor)ThemeConfig.LeftViewController.MenuTable.Background;

            SetupTableView(_menuTableView = new UITableView());

            View.AddSubview(_menuTableView);
        }

        protected override void BindControls()
        {
            NavigationController.NavigationBarHidden = true;

            var set = this.CreateBindingSet<MenuViewController, MenuViewModel>();

            BindTableView(_menuTableView, set);

            set.Apply();

            Mvx.CallbackWhenRegistered<IMvxNavigationService>(async service =>
            {
                await service.Navigate(ViewModel.DefaultViewModel, ((IMvxBundle)new BaseBundle(NavigationType.ClearAndPush)), null);
            });
        }

        #endregion

        #endregion
    }
}

