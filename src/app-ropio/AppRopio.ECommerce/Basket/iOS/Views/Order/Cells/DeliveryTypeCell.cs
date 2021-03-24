using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using ObjCRuntime;
using UIKit;
using AppRopio.ECommerce.Basket.Core.Models;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
    public partial class DeliveryTypeCell : MvxTableViewCell
    {
        protected Models.DeliveryTypeCell CellTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.Cell; } }

        public static readonly NSString Key = new NSString("DeliveryTypeCell");
        public static readonly UINib Nib = UINib.FromName("DeliveryTypeCell", NSBundle.MainBundle);

        protected DeliveryTypeCell(IntPtr handle) : base(handle)
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
            SetupTitleLabel(_titleLabel);
            SetupCheckImageView(_checkImageView);
            SetupDisclosureImageView(_disclosureImageView);
            SetupSeparatorView(_separatorView);

            this.SetupStyle(CellTheme);
        }

        protected virtual void SetupTitleLabel(UILabel titleLabel)
        {
            titleLabel.SetupStyle(CellTheme.TitleLabel);
        }

        protected virtual void SetupCheckImageView(UIImageView checkImageView)
        {
            checkImageView.SetupStyle(CellTheme.CheckImage);
            checkImageView.Hidden = (Mvx.Resolve<IBasketConfigService>().Config.OrderViewType == Core.Enums.OrderViewType.Partial);
        }

        protected virtual void SetupDisclosureImageView(UIImageView disclosureImageView)
        {
            disclosureImageView.SetupStyle(CellTheme.DisclosureImage);
            disclosureImageView.Hidden = (Mvx.Resolve<IBasketConfigService>().Config.OrderViewType == Core.Enums.OrderViewType.Full);
        }

        /// <summary>
        /// Сделано для FullOrderViewController, т.к. там таблица без сепараторов
        /// </summary>
        protected virtual void SetupSeparatorView(UIView separatorView)
        {
            separatorView.BackgroundColor = (UIColor)Theme.ColorPalette.Separator;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<DeliveryTypeCell, IDeliveryTypeItemVM>();

            BindTitleLabel(_titleLabel, set);
            BindCheckImageView(_checkImageView, set);
            BindDisclosureImageView(_disclosureImageView, set);

            set.Apply();
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<DeliveryTypeCell, IDeliveryTypeItemVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Name);
        }

        protected virtual void BindCheckImageView(UIImageView checkImageView, MvxFluentBindingDescriptionSet<DeliveryTypeCell, IDeliveryTypeItemVM> set)
        {
            set.Bind(checkImageView).For("AnimatedVisibility").To(vm => vm.IsSelected).WithConversion("Visibility");
        }

        protected virtual void BindDisclosureImageView(UIImageView disclosureImageView, MvxFluentBindingDescriptionSet<DeliveryTypeCell, IDeliveryTypeItemVM> set)
        {
        }

        #endregion
    }
}
