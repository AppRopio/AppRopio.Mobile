using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment.Items;
using AppRopio.ECommerce.Basket.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Payment.Cells
{
    public partial class PaymentCell : MvxTableViewCell
    {
        protected Models.PaymentCell CellTheme { get { return Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.Payments.Cell; } }

        public static readonly NSString Key = new NSString("PaymentCell");
        public static readonly UINib Nib = UINib.FromName("PaymentCell", NSBundle.MainBundle);

        protected PaymentCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupName(_titleLabel);
            SetupSelectionImage(_checkImageView);

            this.SetupStyle(CellTheme);
        }

        protected virtual void SetupName(UILabel titleLabel)
        {
            titleLabel.SetupStyle(CellTheme.Title);
        }

        protected virtual void SetupSelectionImage(UIImageView checkImageView)
        {
            checkImageView.SetupStyle(CellTheme.CheckImage);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PaymentCell, IPaymentItemVM>();

            BindName(_titleLabel, set);
            BindSelectionImage(_checkImageView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel titleLabel, MvxFluentBindingDescriptionSet<PaymentCell, IPaymentItemVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Title);
        }

        protected virtual void BindSelectionImage(UIImageView checkImageView, MvxFluentBindingDescriptionSet<PaymentCell, IPaymentItemVM> set)
        {
            set.Bind(checkImageView).For("AnimatedVisibility").To(vm => vm.IsSelected).WithConversion("Visibility");
        }

        #endregion
    }
}
