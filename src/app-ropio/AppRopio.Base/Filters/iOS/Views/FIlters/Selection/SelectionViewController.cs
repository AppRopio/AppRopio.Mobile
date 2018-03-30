using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.Filters.iOS.Views.Filters.Selection.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Selection
{
    public partial class SelectionViewController : CommonViewController<IFilterSelectionViewModel>
    {
        private UIBarButtonItem _clearBarButton { get; set; }

        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public SelectionViewController()
            : base("SelectionViewController", null)
        {
        }

        #region Protected

        #region InitializationControls

        protected virtual void SetupSearchBar(BindableSearchBar searchBar)
        {
            searchBar.SetupStyle(ThemeConfig.Filters.Selection.SearchBar);
        }

        protected virtual void SetupTableView(UITableView tableView)
        {
            tableView.RegisterNibForCellReuse(SelectionCell.Nib, SelectionCell.Key);
            tableView.ContentInset = new UIEdgeInsets(0, 0, 76, 0);
        }

        protected virtual void SetupApplyButton(UIButton applyBtn)
        {
            applyBtn.SetupStyle(ThemeConfig.Filters.ApplyButton);
        }

        protected virtual UIBarButtonItem SetupClearButton()
        {
            var clearBarBtn = new UIBarButtonItem("Сбросить", UIBarButtonItemStyle.Plain, null);
            clearBarBtn.SetupStyle(ThemeConfig.Filters.ClearButton);
            return clearBarBtn;
        }

        #endregion

        #region BindingControls

        protected virtual void BindSearchBar(BindableSearchBar searchBar, MvxFluentBindingDescriptionSet<SelectionViewController, IFilterSelectionViewModel> set)
        {
            set.Bind(searchBar).To(vm => vm.SearchText);
            set.Bind(searchBar).For(s => s.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchBar).For(s => s.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<SelectionViewController, IFilterSelectionViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(d => d.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new MvxSimpleTableViewSource(tableView, SelectionCell.Key, SelectionCell.Key);
        }

        protected virtual void BindApplyButton(UIButton applyBtn, MvxFluentBindingDescriptionSet<SelectionViewController, IFilterSelectionViewModel> set)
        {
            set.Bind(applyBtn).To(vm => vm.ApplyCommand);
        }

        protected virtual void BindClearButton(UIBarButtonItem clearBtn, MvxFluentBindingDescriptionSet<SelectionViewController, IFilterSelectionViewModel> set)
        {
            set.Bind(clearBtn).To(vm => vm.ClearCommand);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            SetupSearchBar(_searchBar);

            SetupTableView(_tableView);

            SetupApplyButton(_applyBtn);

            _clearBarButton = SetupClearButton();

            if (_clearBarButton != null)
                NavigationItem.SetRightBarButtonItem(_clearBarButton, false);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<SelectionViewController, IFilterSelectionViewModel>();

            set.Bind(this).For(vc => vc.Title).To(vm => vm.Name);

            BindSearchBar(_searchBar, set);

            BindTableView(_tableView, set);

            BindApplyButton(_applyBtn, set);

            BindClearButton(_clearBarButton, set);

            set.Apply();
        }

        #endregion

        #endregion
    }
}

