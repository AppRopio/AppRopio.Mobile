using System.ComponentModel;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Full;
using AppRopio.ECommerce.Basket.iOS.Controls;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Cells;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Full
{
    public partial class FullOrderViewController : CommonViewController<IFullOrderViewModel>
    {
        private UIView _deliveryPickerView;
        private UIPickerView _deliveryTimePickerView;
        private DeliveryTimePickerViewModel _deliveryTimePickerViewModel;
        private UIButton _applyDeliveryTimeButton;

        private UIButton _accessoryButton;

        protected Models.Order OrderTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order; } }
        protected Models.OrderFieldCell CellTheme { get { return OrderTheme.UserInfo.Cell ?? Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.BaseOrderFieldCell; } }

        public FullOrderViewController()
            : base("FullOrderViewController", null)
        {
        }

        private void ShowDeliveryTimePicker(UIView deliveryPickerView)
        {
            UIView.Animate(0.3, () => deliveryPickerView.ChangeFrame(y: View.Frame.Height - deliveryPickerView.Frame.Height));
        }

        private void HideDeliveryTimePicker(UIView deliveryPickerView)
        {
            UIView.Animate(0.3, () => deliveryPickerView.ChangeFrame(y: View.Frame.Height));
        }

        #region Private 

        private void VMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ViewModel.DeliveryViewModel.IsShowDeliveryTimePicker)) &&
                _tableView != null && _deliveryTimeView != null)
            {
                _deliveryTimeView.Hidden = !ViewModel.DeliveryViewModel.IsShowDeliveryTimePicker;
                _tableView.UpdateFooterHeight();

                if (ViewModel.DeliveryViewModel.IsShowDeliveryTimePicker && ViewModel.CanGoNext)
                    _tableView.ScrollRectToVisible(new CGRect(0, _tableView.TableFooterView.Frame.Top, _tableView.TableFooterView.Frame.Width, _tableView.TableFooterView.Frame.Height), true);
            }
        }

        #endregion

        #region Protected

        #region InitializationControls

        protected virtual void SetupTableView(UITableView tableView)
        {
            RegisterCells(tableView);
            
            tableView.RowHeight = CellTheme.Size.Height ?? 82;
        }

        protected virtual void RegisterCells(UITableView tableView)
        {
            tableView.RegisterNibForHeaderFooterViewReuse(SectionHeader.Nib, SectionHeader.Key);
            tableView.RegisterNibForCellReuse(DeliveryTypeCell.Nib, DeliveryTypeCell.Key);
            tableView.RegisterNibForCellReuse(OrderFieldCell.Nib, OrderFieldCell.Key);
            tableView.RegisterNibForCellReuse(OrderFieldCounterCell.Nib, OrderFieldCounterCell.Key);
            tableView.RegisterNibForCellReuse(OrderFieldOptionalCell.Nib, OrderFieldOptionalCell.Key);
        }

        protected virtual void SetupDeliveryTimeSeparator(UIView deliveryTimeSeparatorView)
        {
            deliveryTimeSeparatorView.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupDeliveryTimeLabel(UILabel deliveryTimeLabel)
        {
            deliveryTimeLabel.SetupStyle(OrderTheme.DeliveryInfo.TimeView.TimeLabel);
            deliveryTimeLabel.Text = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_DeliveryTimeAndDay");
        }

        protected virtual void SetupDeliveryTimePickerView(UIView deliveryPickerView, UIPickerView pickerView)
        {
            pickerView.Frame = new CGRect(0, 50, DeviceInfo.ScreenWidth, 220);
            pickerView.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();

            var accessoryHeight = 50;
            var buttonWidth = 60;
            var buttonPadding = 12;
            var accessoryView = new UIView()
                .WithFrame(0, 0, DeviceInfo.ScreenWidth, accessoryHeight)
                .WithBackground(Theme.ColorPalette.Background.ToUIColor())
                .WithSubviews(
                    new UIButton(UIButtonType.Custom)
                    .WithFrame(buttonPadding, 0, buttonWidth, accessoryHeight)
                    .WithTune(tune =>
                    {
                        tune.SetupStyle(OrderTheme.DeliveryInfo.TimeCancelButton);
                        tune.SetTitle(Mvx.Resolve<ILocalizationService>().GetLocalizableString("Base", "Cnacel"), UIControlState.Normal);
                        tune.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                        tune.TouchUpInside += (sender, e) => HideDeliveryTimePicker(deliveryPickerView);
                    }),
                    new UIButton(UIButtonType.Custom)
                    .WithFrame(DeviceInfo.ScreenWidth - buttonWidth - buttonPadding, 0, buttonWidth, accessoryHeight)
                    .WithTune(tune =>
                    {
                        tune.SetupStyle(OrderTheme.DeliveryInfo.TimeApplyButton);
                        tune.SetTitle(Mvx.Resolve<ILocalizationService>().GetLocalizableString("Base", "Done"), UIControlState.Normal);
                        tune.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                        tune.TouchUpInside += (sender, e) => HideDeliveryTimePicker(deliveryPickerView);

                        _applyDeliveryTimeButton = tune;
                    }),
                    new UIView().WithFrame(0, 0, DeviceInfo.ScreenWidth, 1).WithBackground(Theme.ColorPalette.Separator.ToUIColor()),
                    new UIView().WithFrame(0, accessoryHeight - 1, DeviceInfo.ScreenWidth, 1).WithBackground(Theme.ColorPalette.Separator.ToUIColor())
                );

            deliveryPickerView.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
            deliveryPickerView.ChangeFrame(h: accessoryHeight + pickerView.Frame.Height);

            deliveryPickerView.AddSubviews(accessoryView, pickerView);
        }

        protected virtual void SetupDeliveryTimeIconView(UIImageView iconImageView)
        {
            iconImageView.SetupStyle(OrderTheme.DeliveryInfo.TimeView.TimeIcon);
        }

        protected virtual void SetupDeliveryTimeActivityIndicator(UIActivityIndicatorView activityIndicator)
        {
            activityIndicator.Color = Theme.ColorPalette.Accent.ToUIColor();
        }

        protected virtual void SetupDeliveryPriceTitle(UILabel deliveryPriceTitle)
        {
            deliveryPriceTitle.Text = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_DeliveryPrice");
            deliveryPriceTitle.SetupStyle(OrderTheme.DeliveryPriceLabel);
        }

        protected virtual void SetupDeliveryPriceLabel(UILabel deliveryPriceLabel)
        {
            deliveryPriceLabel.SetupStyle(OrderTheme.DeliveryPriceLabel);
        }

        protected virtual void SetupNextButton(UIButton nextButton)
        {
            nextButton.SetupStyle(OrderTheme.NextButton);
        }

        protected virtual void SetupAccessoryButton(UIButton accessoryNextButton)
        {
            accessoryNextButton.SetTitle(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_Done"), UIControlState.Normal);
            accessoryNextButton.ChangeFrame(w: DeviceInfo.ScreenWidth, h: 44);
            accessoryNextButton.SetupStyle(OrderTheme.AccessoryNextButton);
        }

        protected virtual void SetupDeliveryTimeView(UIView deliveryTimeView)
        {
            deliveryTimeView.SetupStyle(OrderTheme.DeliveryInfo.TimeView);
        }

        protected virtual void SetupBottomView(UIView bottomView)
        {
            bottomView.SetupStyle(OrderTheme.BottomView);
        }

        #endregion

        #region BindingControls

        protected virtual FullOrderTableSource SetupTableViewDataSource(UITableView tableView)
        {
            var source = new FullOrderTableSource(tableView);

            source.FieldInputAccessoryView = _accessoryButton;

            return source;
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<FullOrderViewController, IFullOrderViewModel> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            set.Bind(dataSource).For(s => s.ItemsSource).To(vm => vm.Items);
            set.Bind(dataSource).For(s => s.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual void BindDeliveryTimeLabel(UIView deliveryPickerView, UILabel deliveryTimeLabel, MvxFluentBindingDescriptionSet<FullOrderViewController, IFullOrderViewModel> set)
        {
            set.Bind(deliveryTimeLabel).To(vm => vm.DeliveryViewModel.SelectedDeliveryTimeValue).OneWay();

            _deliveryTimeView.ActionOnTap(() => ShowDeliveryTimePicker(deliveryPickerView));
            set.Bind(_deliveryTimeView).For("Visibility").To(vm => vm.DeliveryViewModel.IsShowDeliveryTimePicker).WithConversion("Visibility");
        }

        protected virtual void BindDeliveryTimePickerView(MvxPickerViewModel deliveryTimePickerViewModel, UIButton applyButton, MvxFluentBindingDescriptionSet<FullOrderViewController, IFullOrderViewModel> set)
        {
            set.Bind(deliveryTimePickerViewModel).For(s => s.ItemsSource).To(vm => vm.DeliveryViewModel.DaysItems);

            //set.Bind(applyButton).To(vm => vm.ApplyDeliveryTimeCommand);
            applyButton.TouchUpInside += (sender, e) =>
            {
                if (ViewModel.DeliveryViewModel.ApplyDeliveryTimeCommand.CanExecute(deliveryTimePickerViewModel.SelectedItem))
                    ViewModel.DeliveryViewModel.ApplyDeliveryTimeCommand.Execute(deliveryTimePickerViewModel.SelectedItem);
            };
        }

        protected virtual void BindDeliveryTimeActivityIndicator(UIActivityIndicatorView activityIndicator, MvxFluentBindingDescriptionSet<FullOrderViewController, IFullOrderViewModel> set)
        {
            set.Bind(activityIndicator).For("AnimatedVisibility").To(vm => vm.DeliveryViewModel.DeliveryTimeLoading).WithConversion("Visibility");
        }

        protected virtual void BindDeliveryPriceLabel(UILabel deliveryPriceLabel, MvxFluentBindingDescriptionSet<FullOrderViewController, IFullOrderViewModel> set)
        {
            set.Bind(deliveryPriceLabel).To(vm => vm.DeliveryViewModel.DeliveryPrice).WithConversion("StringFormat", new StringFormatParameter
            {
                StringFormat = (arg) =>
                {
                    if (arg == null) {
                        string deliveryEmptyText = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_DeliveryEmptyText");
                        if (!string.IsNullOrEmpty(deliveryEmptyText)) {
                            return deliveryEmptyText;
                        }
                        return $"{LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_DeliveryPriceFrom")} {0.0m.ToString(AppSettings.CurrencyFormat, AppSettings.SettingsCulture.NumberFormat)}";
                    }
                    var d = (decimal)arg;
                    if (d == 0.0m) {
                        return LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_Free");
                    } else if (d > 0.0m) {
                        return d.ToString(AppSettings.CurrencyFormat, AppSettings.SettingsCulture.NumberFormat);
                    } else {
                        return string.Empty;
                    }
                }
            });
        }

        protected virtual void BindNextButton(UIButton nextButton, MvxFluentBindingDescriptionSet<FullOrderViewController, IFullOrderViewModel> set)
        {
            set.Bind(nextButton)
               .For("Title")
               .To(vm => vm.DeliveryViewModel.Amount)
               .WithConversion(
                   "StringFormat",
                   new StringFormatParameter
                   {
                       StringFormat = (arg) =>
                       {
                            var str = $"{LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_OrderBy")} {((decimal)arg).ToString(AppSettings.CurrencyFormat, AppSettings.SettingsCulture.NumberFormat)}";
                            return OrderTheme.NextButton.UppercaseTitle ? str.ToUpperInvariant() : str;
                       }
                   });

            set.Bind(nextButton).To(vm => vm.NextCommand);
            set.Bind(nextButton).For(s => s.Enabled).To(vm => vm.CanGoNext);
        }

        protected virtual void BindAccessoryButton(UIButton accessoryNextButton, MvxFluentBindingDescriptionSet<FullOrderViewController, IFullOrderViewModel> set)
        {
            accessoryNextButton.TouchUpInside += (sender, e) => View.EndEditing(true);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_Title");

            RegisterKeyboardActions = true;

            SetupTableView(_tableView);

            SetupDeliveryTimeSeparator(_deliveryTimeSeparatorView);
            SetupDeliveryTimeLabel(_deliveryTimeLabel);
            SetupDeliveryTimeIconView(_deliveryTimeIconView);
            SetupDeliveryTimeActivityIndicator(_deliveryTimeActivityIndicator);

            SetupDeliveryPriceTitle(_deliveryPriceTitle);
            SetupDeliveryPriceLabel(_deliveryPriceLabel);
            SetupNextButton(_nextButton);
            SetupAccessoryButton(_accessoryButton ?? (_accessoryButton = new UIButton()));

            _deliveryTimePickerView = new UIPickerView();
            _deliveryTimePickerViewModel = new DeliveryTimePickerViewModel(_deliveryTimePickerView, OrderTheme.DeliveryInfo.TimeView.TimeCell);
            _deliveryTimePickerView.Model = _deliveryTimePickerViewModel;

            SetupDeliveryTimePickerView(
                (_deliveryPickerView = new UIView(new CGRect(0, DeviceInfo.ScreenHeight, DeviceInfo.ScreenWidth, 0))),
                _deliveryTimePickerView
            );

            View.AddSubview(_deliveryPickerView);

            SetupBottomView(_bottomView);

            SetupDeliveryTimeView(_deliveryTimeView);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<FullOrderViewController, IFullOrderViewModel>();

            ViewModel.DeliveryViewModel.PropertyChanged += VMPropertyChanged;

            BindTableView(_tableView, set);

            BindDeliveryTimeLabel(_deliveryPickerView, _deliveryTimeLabel, set);
            BindDeliveryTimePickerView(_deliveryTimePickerViewModel, _applyDeliveryTimeButton, set);
            BindDeliveryTimeActivityIndicator(_deliveryTimeActivityIndicator, set);

            BindDeliveryPriceLabel(_deliveryPriceLabel, set);
            BindNextButton(_nextButton, set);

            BindAccessoryButton(_accessoryButton, set);

            set.Apply();

            _tableView.UpdateFooterHeight();
        }

        protected override void CleanUp()
        {
            if (ViewModel != null && ViewModel.DeliveryViewModel != null)
                ViewModel.DeliveryViewModel.PropertyChanged -= VMPropertyChanged;

            ReleaseDesignerOutlets();
        }

        #endregion

        #endregion
    }
}

