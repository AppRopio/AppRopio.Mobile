using System;
using AppRopio.ECommerce.Basket.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
    public abstract class OrderFieldBaseCell : MvxTableViewCell
    {
        protected Models.OrderFieldCell CellTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.BaseOrderFieldCell; } }

        public UIView FieldInputAccessoryView { get; set; }

        protected OrderFieldBaseCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        protected abstract void InitializeControls();

        protected abstract void BindControls();
    }
}
