using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Selection.Cells;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;
using AppRopio.ECommerce.Products.Core;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Selection
{
    public partial class PDSelectionViewController : CommonViewController<IProductDetailsSelectionViewModel>
    {
        private UIBarButtonItem _clearBarButton { get; set; }

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public PDSelectionViewController()
            : base("PDSelectionViewController", null)
        {
        }

        #region Protected

        #region InitializationControls

        protected virtual void SetupSearchBar(BindableSearchBar searchBar)
        {
            searchBar.SetupStyle(ThemeConfig.ProductDetails.Selection.SearchBar);
        }

        protected virtual void SetupTableView(UITableView tableView)
        {
            tableView.RegisterNibForCellReuse(PDSelectionCell.Nib, PDSelectionCell.Key);
            tableView.ContentInset = new UIEdgeInsets(0, 0, 76, 0);
        }

        protected virtual void SetupApplyButton(UIButton applyBtn)
        {
            applyBtn.SetupStyle(ThemeConfig.ProductDetails.Selection.ApplyButton);
        }

        protected virtual UIBarButtonItem SetupClearButton()
        {
            var clearBarBtn = new UIBarButtonItem(LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "Selection_Clear"), UIBarButtonItemStyle.Plain, null);
            clearBarBtn.SetupStyle(ThemeConfig.ProductDetails.Selection.ClearButton);
            return clearBarBtn;
        }

        #endregion

        #region BindingControls

        protected virtual void BindSearchBar(BindableSearchBar searchBar, MvxFluentBindingDescriptionSet<PDSelectionViewController, IProductDetailsSelectionViewModel> set)
        {
            set.Bind(searchBar).To(vm => vm.SearchText);
            set.Bind(searchBar).For(s => s.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchBar).For(s => s.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<PDSelectionViewController, IProductDetailsSelectionViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(d => d.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new MvxSimpleTableViewSource(tableView, PDSelectionCell.Key, PDSelectionCell.Key);
        }

        protected virtual void BindApplyButton(UIButton applyBtn, MvxFluentBindingDescriptionSet<PDSelectionViewController, IProductDetailsSelectionViewModel> set)
        {
            set.Bind(applyBtn).To(vm => vm.ApplyCommand);
            set.Bind(applyBtn).For("Title").To(vm => vm.ApplyTitle);
        }

        protected virtual void BindClearButton(UIBarButtonItem clearBtn, MvxFluentBindingDescriptionSet<PDSelectionViewController, IProductDetailsSelectionViewModel> set)
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
            var set = this.CreateBindingSet<PDSelectionViewController, IProductDetailsSelectionViewModel>();

            set.Bind(this).For(vc => vc.Title).To(vm => vm.Name);

            BindSearchBar(_searchBar, set);

            BindTableView(_tableView, set);

            BindApplyButton(_applyBtn, set);

            BindClearButton(_clearBarButton, set);

            set.Apply();
        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();

            base.CleanUp();
        }

        #endregion

        #endregion
    }
}

