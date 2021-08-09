using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Basket.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
    public partial class OrderFieldAutocompleteCell : MvxTableViewCell
    {
        protected Models.AutocompleteCell CellTheme { get { return Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.AutocompleteCell; } }

        public static readonly NSString Key = new NSString("OrderFieldAutocompleteCell");
        public static readonly UINib Nib = UINib.FromName("OrderFieldAutocompleteCell", NSBundle.MainBundle);

        protected OrderFieldAutocompleteCell(IntPtr handle) : base(handle)
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
            SetupValueLabel(_valueLabel);

            this.SetupStyle(CellTheme);
        }

        protected virtual void SetupValueLabel(UILabel valueLabel)
        {
            valueLabel.SetupStyle(CellTheme.Title);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<OrderFieldAutocompleteCell, IOrderFieldAutocompleteItemVM>();

            BindValueLabel(_valueLabel, set);

            set.Apply();
        }

        protected virtual void BindValueLabel(UILabel valueLabel, MvxFluentBindingDescriptionSet<OrderFieldAutocompleteCell, IOrderFieldAutocompleteItemVM> set)
        {
            set.Bind(valueLabel).To(vm => vm.Value);
        }

        #endregion
    }
}
