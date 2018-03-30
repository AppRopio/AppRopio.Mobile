using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.Base.Map.Core.ViewModels.Points.List;
using AppRopio.Base.Map.iOS.Services;
using AppRopio.Base.Map.iOS.Views.Points.List.Cells;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Map.iOS.Views.Points.List
{
    public partial class PointsListViewController : CommonViewController<IPointsListViewModel>
    {
        private BindableSearchBar _searchBar;
        private BindableSearchController _searchController;

        private UIBarButtonItem _mapBarButton;

        protected Models.PointsList PointsListTheme { get { return Mvx.Resolve<IMapThemeConfigService>().ThemeConfig.Points.List; } }

        public PointsListViewController() 
            : base("PointsListViewController", null)
        {
            
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = "Адреса";

            _searchController = new BindableSearchController(searchResultsController: null);

            DefinesPresentationContext = true;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                _searchController = new BindableSearchController(searchResultsController: null);

                NavigationItem.HidesSearchBarWhenScrolling = true;
                NavigationItem.SearchController = _searchController;

                SetupSearchBar(_searchController.SearchBar);
            }
            else
            {
                _searchBar = new BindableSearchBar(new CGRect(8, 0, DeviceInfo.ScreenWidth - 16, 44));

                SetupSearchBar(_searchBar);

                _tableView.TableHeaderView = new UIView()
                    .WithFrame(0, 0, DeviceInfo.ScreenWidth, 44)
                    .WithSubviews(_searchBar);
            }

            SetupTabelView(_tableView);

            _mapBarButton = SetupMapButton();

            NavigationItem.SetRightBarButtonItem(_mapBarButton, false);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<PointsListViewController, IPointsListViewModel>();

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                BindSearchController(_searchController, set);
            else
                BindSearchBar(_searchBar, set);

            BindTableView(_tableView, set);
            BindMapButton(_mapBarButton, set);

            set.Apply();
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            ReleaseDesignerOutlets();

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && NavigationItem.SearchController != null)
                NavigationItem.SearchController = null;
        }

        #endregion

        #region InitializationControls

        protected virtual void SetupSearchBar(UISearchBar searchBar)
        {
            searchBar.SetupStyle(PointsListTheme.SearchBar);
        }

        protected virtual void SetupTabelView(UITableView tableView)
        {
            tableView.TableFooterView = new UIView();
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            tableView.SeparatorColor = (UIColor)Theme.ColorPalette.Separator;

            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = PointsListTheme.Cell.Size.Height ?? 200;
        }

        protected virtual UIBarButtonItem SetupMapButton()
        {
            return new UIBarButtonItem(UIImage.FromFile(PointsListTheme.MapImage.Path), UIBarButtonItemStyle.Plain, null);
        }

        #endregion

        #region BindingControls

        protected virtual BaseTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new BaseTableViewSource(tableView, PointCell.Key, PointCell.Key);
        }

        protected virtual void BindSearchController(BindableSearchController searchController, MvxFluentBindingDescriptionSet<PointsListViewController, IPointsListViewModel> set)
        {
            set.Bind(searchController.SearchBar).To(vm => vm.SearchText);
            set.Bind(searchController).For(sb => sb.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchController).For(sb => sb.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindSearchBar(BindableSearchBar searchBar, MvxFluentBindingDescriptionSet<PointsListViewController, IPointsListViewModel> set)
        {
            set.Bind(searchBar).To(vm => vm.SearchText);
            set.Bind(searchBar).For(sb => sb.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchBar).For(sb => sb.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<PointsListViewController, IPointsListViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            set.Bind(dataSource).For(s => s.ItemsSource).To(vm => vm.Items);
            set.Bind(dataSource).For(s => s.LoadMoreCommand).To(vm => vm.LoadMoreCommand);
            //set.Bind(dataSource).For(s => s.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual void BindMapButton(UIBarButtonItem mapBarButton, MvxFluentBindingDescriptionSet<PointsListViewController, IPointsListViewModel> set)
        {
            set.Bind(mapBarButton).To(vm => vm.MapCommand);
        }

        #endregion

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _searchController?.RemoveBottomSeparator();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _searchController?.RemoveBottomSeparator();
        }
    }
}

