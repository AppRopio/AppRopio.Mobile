using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.FlowLayouts;
using AppRopio.Base.iOS.Models.ValueConverters;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.Autocomplete;
using AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.Hint;
using AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.History;
using AppRopio.ECommerce.Products.iOS.Views.ContentSearch.ViewSources;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using AppRopio.ECommerce.Products.Core;

namespace AppRopio.ECommerce.Products.iOS.Views.ContentSearch
{
    public partial class ContentSearchViewController : CommonViewController<IContentSearchViewModel>, IPinterestLayoutDelegate
    {
        private BindableSearchBar _searchBar;
        private BindableSearchController _searchController;
        private UIView _searchBarView;

        private MvxViewController _contentViewController;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        private bool _keyboardHidden;
        public bool KeyboardHidden
        {
            get
            {
                return _keyboardHidden;
            }
            set
            {
                _keyboardHidden = value;
                if (value)
                {
                    _searchController?.SearchBar?.EndEditing(true);
                    _searchBar?.EndEditing(true);
                }
            }
        }

        public ContentSearchViewController()
            : base("ContentSearchViewController", null)
        {
            AutomaticallyLargeTitleDisplayMode = false;
        }

        #region Private

        private void OnTableViewScrollStarted(object sender, EventArgs e)
        {
            _searchController?.SearchBar?.EndEditing(true);
            _searchBar?.EndEditing(true);
        }

        #endregion

        #region Protected

        #region InitializationControls

        protected virtual void SetupSearchBar(UISearchBar searchBar)
        {
            searchBar.SetupStyle(ThemeConfig.ContentSearch.SearchBar);
        }

        protected virtual void SetupHistoryTableView(UITableView historyTableView)
        {
            historyTableView.SeparatorColor = Theme.ColorPalette.Separator.ToUIColor();
            historyTableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
            historyTableView.SeparatorInset = new UIEdgeInsets(0, 16, 0, 0);

            historyTableView.ContentInset = new UIEdgeInsets(0, 0, 76, 0);

            historyTableView.RegisterNibForCellReuse(HistoryCell.Nib, HistoryCell.Key);
            historyTableView.RowHeight = HistoryCell.CONTENT_HEIGHT;

            historyTableView.TableFooterView = new UIView();
        }

        protected virtual void SetupClearHistoryBtn(UIButton clearHistoryBtn)
        {
            clearHistoryBtn.SetupStyle(ThemeConfig.ContentSearch.ClearHistoryButton);
        }

        protected virtual void SetupAutocompleteCollection(UICollectionView autocompleteCollectionView)
        {
            var flowLayout = (autocompleteCollectionView.CollectionViewLayout as PinterestLayout);
            flowLayout.Delegate = this;
            if (flowLayout != null)
            {
                flowLayout.MinimumLineSpacing = 8;
                flowLayout.MinimumInteritemSpacing = 8;

                var maxWidth = (DeviceInfo.ScreenWidth - 48) / 2;
                flowLayout.ItemSize = new CGSize(maxWidth, AutocompleteCell.CONTENT_HEIGHT);
            }

            autocompleteCollectionView.ContentInset = new UIEdgeInsets(2, 4, 2, 4);

            autocompleteCollectionView.ScrollEnabled = false;
            autocompleteCollectionView.RegisterNibForCell(AutocompleteCell.Nib, AutocompleteCell.Key);
        }

        protected virtual void SetupHintsTableView(UITableView hintsTableView)
        {
            hintsTableView.SeparatorColor = Theme.ColorPalette.Separator.ToUIColor();
            hintsTableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
            hintsTableView.SeparatorInset = new UIEdgeInsets(0, 16, 0, 0);

            hintsTableView.RegisterNibForCellReuse(HintCell.Nib, HintCell.Key);
            hintsTableView.RowHeight = HintCell.CONTENT_HEIGHT;

            hintsTableView.TableFooterView = new UIView();
        }

        #endregion

        #region BindingControls

        protected virtual void BindSearchController(BindableSearchController searchController, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            set.Bind(searchController.SearchBar).To(vm => vm.SearchText);
            set.Bind(searchController).For(s => s.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchController).For(s => s.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindSearchBar(BindableSearchBar searchBar, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            set.Bind(searchBar).To(vm => vm.SearchText);
            set.Bind(searchBar).For(s => s.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchBar).For(s => s.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindContentView(UIView contentView, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            set.Bind(contentView).For("Visibility").To(vm => vm.ContentVisible).WithConversion("Visibility");
        }

        protected virtual void BindHistoryView(UIView historyView, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            set.Bind(historyView).For("Visibility").To(vm => vm.HistoryVisible).WithConversion("Visibility");
        }

        protected virtual void BindHistoryTableView(UITableView historyTableView, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            var dataSource = SetupHistoryDataSource(historyTableView);
            historyTableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.HistoryItems);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.HistorySelectionCommand);

            set.Bind(historyTableView).For("Visibility").To(vm => vm.HistoryItems.Count).WithConversion("Visibility");

            historyTableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupHistoryDataSource(UITableView historyTableView)
        {
            var dataSource = new ScrolledEventsTableViewSource(historyTableView, HistoryCell.Key);

            dataSource.OnDraggingStarted += OnTableViewScrollStarted;
            dataSource.OnDecelerationStarted += OnTableViewScrollStarted;

            return dataSource;
        }

        protected virtual void BindClearHistoryBtn(UIButton clearHistoryBtn, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            set.Bind(clearHistoryBtn).To(vm => vm.ClearHistoryCommand);
            set.Bind(clearHistoryBtn).For("Title").To(vm => vm.ClearHistoryTitle);
            set.Bind(clearHistoryBtn).For("Visibility").To(vm => vm.HistoryItems.Count).WithConversion("Visibility");
        }

        protected virtual void BindHintsView(UIView hintsView, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            set.Bind(hintsView).For("Visibility").To(vm => vm.HintsVisible).WithConversion("Visibility");
        }

        protected virtual void BindAutocompleteCollection(UICollectionView autocompleteCollectionView, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            var dataSource = SetupAutocompleteDataSource(autocompleteCollectionView);
            autocompleteCollectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.AutocomleteItems);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.AutocompleteSelectionCommand);

            autocompleteCollectionView.ReloadData();
        }

        protected virtual MvxCollectionViewSource SetupAutocompleteDataSource(UICollectionView autocompleteCollectionView)
        {
            return new AutocompleteCollectionViewSource(autocompleteCollectionView, AutocompleteCell.Key);
        }

        protected virtual void BindHintsTableView(UITableView hintsTableView, MvxFluentBindingDescriptionSet<ContentSearchViewController, IContentSearchViewModel> set)
        {
            var dataSource = SetupHintsDataSource(hintsTableView);
            hintsTableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.HintsItems);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.HintSelectionCommand);

            set.Bind(hintsTableView).For(v => v.TableHeaderView).To(vm => vm.AutocomleteItems.Count).WithConversion("SizeVisibility", new SizeVisibilityParameter { View = hintsTableView.TableHeaderView, MaximumHeight = () => 90, MinimumHeight = () => 0 });
            set.Bind(hintsTableView).For("Visibility").To(vm => vm.HintsItems.Count).WithConversion("Visibility");

            hintsTableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupHintsDataSource(UITableView hintsTableView)
        {
            var dataSource = new ScrolledEventsTableViewSource(hintsTableView, HintCell.Key);

            dataSource.OnDraggingStarted += OnTableViewScrollStarted;
            dataSource.OnDecelerationStarted += OnTableViewScrollStarted;

            return dataSource;
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            _contentViewController = this.CreateViewControllerFor(ViewModel.ContentVm) as MvxViewController;
            _contentViewController.ViewModel = ViewModel.ContentVm;

            DefinesPresentationContext = true;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                _searchController = new BindableSearchController(searchResultsController: null);

                NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Always;
                NavigationItem.Title = LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "ContentSearch_Title");
                NavigationItem.HidesSearchBarWhenScrolling = false;
                NavigationItem.SearchController = _searchController;

                SetupSearchBar(_searchController.SearchBar);
            }
            else
            {
                _searchBarView = new UIView(new CGRect(UIDevice.CurrentDevice.CheckSystemVersion(11, 0) ? 6 : -21, 0, DeviceInfo.ScreenWidth - 50, 44));

                NavigationItem.TitleView = new UIView()
                .WithBackground(UIColor.Clear)
                .WithFrame(0, 0, DeviceInfo.ScreenWidth, 44)
                .WithSubviews(
                    _searchBarView.WithSubviews(_searchBar = new BindableSearchBar(new CGRect(8, 0, DeviceInfo.ScreenWidth - 66, 44)))
                );

                SetupSearchBar(_searchBar);
            }

            SetupHistoryTableView(_historyTableView);
            SetupClearHistoryBtn(_clearHistoryBtn);

            SetupAutocompleteCollection(_autocompleteCollectionView);
            SetupHintsTableView(_hintsTableView);
        }

        protected override void BindControls()
        {
            AddChildViewController(_contentViewController);

            _contentViewController.View.TranslatesAutoresizingMaskIntoConstraints = false;

            View.AddSubview(_contentViewController.View);

            View.AddConstraints(new[]
            {
                NSLayoutConstraint.Create(_contentViewController.View, NSLayoutAttribute.Top, NSLayoutRelation.Equal, View, NSLayoutAttribute.Top, 1, 0),
                NSLayoutConstraint.Create(_contentViewController.View, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, View, NSLayoutAttribute.Trailing, 1, 0),
                NSLayoutConstraint.Create(_contentViewController.View, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, View, NSLayoutAttribute.Leading, 1, 0),
                NSLayoutConstraint.Create(_contentViewController.View, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1, 0)
            });

            var set = this.CreateBindingSet<ContentSearchViewController, IContentSearchViewModel>();

            set.Bind().For(v => v.KeyboardHidden).To(vm => vm.ContentVisible);

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                BindSearchController(_searchController, set);
            else
                BindSearchBar(_searchBar, set);

            BindContentView(_contentViewController.View, set);

            BindHistoryView(_historyView, set);
            BindHistoryTableView(_historyTableView, set);
            BindClearHistoryBtn(_clearHistoryBtn, set);

            BindHintsView(_hintsView, set);
            BindAutocompleteCollection(_autocompleteCollectionView, set);
            BindHintsTableView(_hintsTableView, set);

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

        #endregion

        #region Public

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_searchBarView != null)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    _searchBarView.ChangeFrame(x: 6);
                else
                    _searchBarView.ChangeFrame(x: -21);

                NavigationItem.TitleView?.AddSubview(_searchBarView);
            }

            _searchController?.RemoveBottomSeparator();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _searchController?.RemoveBottomSeparator();

            if (_searchBarView != null)
            {
                _searchBarView.RemoveFromSuperview();
                _searchBarView.ChangeFrame(x: 40);

                if (NavigationController != null && NavigationController.NavigationBar != null)
                {
                    NavigationController.NavigationBar.AddSubview(_searchBarView);
                }
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            if (_searchBarView != null)
            {
                _searchBarView.RemoveFromSuperview();

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    _searchBarView.ChangeFrame(x: 6);
                else
                    _searchBarView.ChangeFrame(x: -21);

                if (NavigationItem != null)
                    NavigationItem.TitleView?.AddSubview(_searchBarView);
            }

            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            if (_searchBarView != null)
                _searchBarView.RemoveFromSuperview();

            base.ViewDidDisappear(animated);
        }

        #region IPinterestLayout implementation

        public CGSize SizeForCellAtIndexPath(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return (collectionView.Source as AutocompleteCollectionViewSource).GetSizeForItem(collectionView, collectionView.CollectionViewLayout, indexPath);
        }

        #endregion

        #endregion
    }
}

