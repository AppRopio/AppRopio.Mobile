
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.Filters.Core.ViewModels.Filters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Messages;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Vertical;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.MinMax.Date;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.MinMax.Number;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Picker;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Selection.MultiSelection;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Selection.OneSelection;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using UIKit;
using AppRopio.Base.Filters.iOS.Views.FIlters.Cells.Switch;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Filters.Core;

namespace AppRopio.Base.Filters.iOS.Views.Filters
{
    public partial class FiltersViewController : CommonViewController<IFiltersViewModel>
    {
        private UIBarButtonItem _clearBarButton;

        private MvxSubscriptionToken _subscriptionToken;

        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        protected IMvxMessenger MessengerService { get { return Mvx.Resolve<IMvxMessenger>(); } }

        public FiltersViewController()
            : base("FiltersViewController", null)
        {
            RegisterKeyboardActions = true;
        }

        #region Private

        private void OnFiltersReloadMessageRecieved(FiltersReloadWhenValueChangedMessage msg)
        {
            (_tableView.Source as FiltersTableViewSource)?.ClearCachedHeights();
            
            _tableView.BeginUpdates();
            _tableView.ReloadData();
            _tableView.EndUpdates();

            var selectedIndexPath = (_tableView.Source as FiltersTableViewSource).IndexPathForSelectedRow;
            if (selectedIndexPath != null)
                NSOperationQueue.MainQueue.AddOperation(() => _tableView.ScrollToRow(selectedIndexPath, UITableViewScrollPosition.Middle, true));
        }

        private void RegisterCells(UITableView tableView)
        {
            tableView.RegisterNibForCellReuse(HorizontalCollectionCell.Nib, HorizontalCollectionCell.Key);
            tableView.RegisterNibForCellReuse(VerticalCollectionCell.Nib, VerticalCollectionCell.Key);
            tableView.RegisterNibForCellReuse(DateMinMaxCell.Nib, DateMinMaxCell.Key);
            tableView.RegisterNibForCellReuse(NumberMinMaxCell.Nib, NumberMinMaxCell.Key);
            tableView.RegisterNibForCellReuse(PickerCell.Nib, PickerCell.Key);
            tableView.RegisterNibForCellReuse(MultiSelectionCell.Nib, MultiSelectionCell.Key);
            tableView.RegisterNibForCellReuse(OneSelectionCell.Nib, OneSelectionCell.Key);
            tableView.RegisterNibForCellReuse(SwitchCell.Nib, SwitchCell.Key);
        }

        #endregion

        #region Protected

        #region InitializationControls

        protected virtual void SetupTableView(UITableView tableView)
        {
            RegisterCells(tableView);
            tableView.ContentInset = new UIEdgeInsets(0, 0, 76, 0);
        }

        protected virtual UIBarButtonItem SetupClearButton()
        {
            var clearBarBtn = new UIBarButtonItem(LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Filters_Clear"), UIBarButtonItemStyle.Plain, null);
            clearBarBtn.SetupStyle(ThemeConfig.Filters.ClearButton);
            return clearBarBtn;
        }

        protected virtual void SetupApplyButton(UIButton applyBtn)
        {
            applyBtn.SetupStyle(ThemeConfig.Filters.ApplyButton);
        }

        #endregion

        #region BindingControls

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<FiltersViewController, IFiltersViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.ReloadData();
        }

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new FiltersTableViewSource(tableView);
        }

        protected virtual void BindClearButton(UIBarButtonItem clearBarBtn, MvxFluentBindingDescriptionSet<FiltersViewController, IFiltersViewModel> set)
        {
            set.Bind(clearBarBtn).To(vm => vm.ClearCommand);
        }

        protected virtual void BindApplyButton(UIButton applyBtn, MvxFluentBindingDescriptionSet<FiltersViewController, IFiltersViewModel> set)
        {
            set.Bind(applyBtn).To(vm => vm.ApplyCommand);
            set.Bind(applyBtn).For("Title").To(vm => vm.ApplyTitle);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Filters_Title");

            SetupTableView(_tableView);
            _clearBarButton = SetupClearButton();
            SetupApplyButton(_applyBtn);

            if (_clearBarButton != null)
                NavigationItem.SetRightBarButtonItem(_clearBarButton, false);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<FiltersViewController, IFiltersViewModel>();

            BindTableView(_tableView, set);
            BindClearButton(_clearBarButton, set);
            BindApplyButton(_applyBtn, set);

            set.Apply();

            _subscriptionToken = MessengerService.Subscribe<FiltersReloadWhenValueChangedMessage>(OnFiltersReloadMessageRecieved);
        }

        protected override void CleanUp()
        {
            if (_subscriptionToken != null)
            {
                MessengerService.Unsubscribe<FiltersReloadWhenValueChangedMessage>(_subscriptionToken);
                _subscriptionToken = null;
            }

            ReleaseDesignerOutlets();
        }

        #endregion

        #endregion

        #region Public

        public override void ViewWillDisappear(bool animated)
        {
            View.EndEditing(true);

            base.ViewWillDisappear(animated);
        }

        #endregion
    }
}

