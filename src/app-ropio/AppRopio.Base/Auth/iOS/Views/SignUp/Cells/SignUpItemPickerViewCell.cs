using System;
using System.Linq;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using AppRopio.Base.Auth.iOS.Models;
using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Controls;
using AppRopio.Base.iOS.UIExtentions;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Views.SignUp.Cells
{
    public partial class SignUpItemPickerViewCell : MvxTableViewCell
    {
        protected AuthThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("SignUpItemPickerViewCell");
        public static readonly UINib Nib = UINib.FromName("SignUpItemPickerViewCell", NSBundle.MainBundle);

        protected UIPickerView _pickerView;

        public ARTextField TextField
        {
            get
            {
                return _textField;
            }
            set
            {
                _textField = value;
            }
        }

        protected SignUpItemPickerViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region Private

        private class PickerViewModel : MvxPickerViewModel
        {
            protected AuthThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

            public PickerViewModel(UIPickerView pickerView)
                : base(pickerView)
            {

            }

            public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
            {
                var item = ItemsSource.ElementAt((int)row) as string;
                var label = new AppRopio.Base.iOS.Controls.ARLabel()
                    .WithFrame(0, 0, pickerView.Bounds.Width, GetRowHeight(pickerView, component))
                    .WithTune(tune =>
                    {
                        tune.Text = item;
                        tune.TextAlignment = UITextAlignment.Center;
                    });

                label.SetupStyle(ThemeConfig.PickerCell.Value);

                return label;
            }

            public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
            {
                return (nfloat)ThemeConfig.PickerCell.Size.Height;
            }
        }

        #endregion

        #region Protected

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupTextField(_textField);
            SetupPicker(_pickerView = new UIPickerView(), _textField);
        }

        protected virtual void SetupTextField(UITextField textField)
        {
            textField.SetupStyle(ThemeConfig.TextField);
        }

        protected virtual void SetupPicker(UIPickerView pickerView, UITextField textField)
        {
            pickerView.Frame = new CGRect(0, 0, DeviceInfo.ScreenWidth, 220);
            pickerView.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
            pickerView.ShowSelectionIndicator = true;

            var accessoryView = new UIView()
                .WithFrame(0, 0, DeviceInfo.ScreenWidth, 44)
                .WithBackground(Theme.ColorPalette.Background.ToUIColor())
                .WithSubviews(
                    new UIButton(UIButtonType.Custom)
                    .WithFrame(DeviceInfo.ScreenWidth - 90, 0, 90, 44)
                    .WithTune(tune =>
                    {
                        tune.SetupStyle(ThemeConfig.TextButton);
                        tune.SetTitle(Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString("Base", "Done"), UIControlState.Normal);
                        tune.TouchUpInside += (sender, e) => TextField?.EndEditing(true);
                    }),
                    new UIView().WithFrame(0, 0, DeviceInfo.ScreenWidth, 1).WithBackground(Theme.ColorPalette.Separator.ToUIColor()),
                    new UIView().WithFrame(0, 43, DeviceInfo.ScreenWidth, 1).WithBackground(Theme.ColorPalette.Separator.ToUIColor())
                );

            textField.InputView = pickerView;
            textField.InputAccessoryView = accessoryView;

            textField.ShouldBeginEditing += (tF) =>
            {
                var model = DataContext as SignUpItemPickerViewModel;
                if (model != null)
                {
                    int index = (int)pickerView.SelectedRowInComponent(0);
                    if (index >= 0 && index < model.Items?.Count)
                        model.Value = model.Items[index];

                }
                return true;
            };

            textField.EditingChanged += (sender, e) =>
            {
                textField.Text = (DataContext as SignUpItemPickerViewModel)?.Value ?? string.Empty;
            };
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<SignUpItemPickerViewCell, SignUpItemPickerViewModel>();

            BindTextField(_textField, set);

            BindPickerView(_pickerView, set);

            set.Apply();
        }

        protected virtual void BindPickerView(UIPickerView pickerView, MvxFluentBindingDescriptionSet<SignUpItemPickerViewCell, SignUpItemPickerViewModel> set)
        {
            var bindablePickerViewModel = new PickerViewModel(pickerView);

            set.Bind(bindablePickerViewModel).For(p => p.SelectedItem).To(vm => vm.Value);
            set.Bind(bindablePickerViewModel).For(p => p.ItemsSource).To(vm => vm.Items);

            pickerView.Model = bindablePickerViewModel;
        }

        protected virtual void BindTextField(ARTextField textField, MvxFluentBindingDescriptionSet<SignUpItemPickerViewCell, SignUpItemPickerViewModel> set)
        {
            set.Bind(textField).To(vm => vm.Value).OneWay();
            set.Bind(textField)
               .For(p => p.Error)
               .To(vm => vm.Invalid);
        }

        #endregion

        #endregion

    }
}
