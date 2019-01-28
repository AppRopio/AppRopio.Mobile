using System;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels.CatalogCard;
using AppRopio.ECommerce.Basket.iOS.Models;
using AppRopio.ECommerce.Basket.iOS.Services;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using ObjCRuntime;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.CatalogCard
{
    public partial class BasketCatalogItemView : MvxView, IMvxIosView<IBasketCatalogItemVM>
    {
        #region Fields

        #endregion

        #region Properties

        protected BasketThemeConfig ThemeConfig { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig; } }

        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (IBasketCatalogItemVM)value;
            }
        }

        private MvxViewModelRequest _request;
        public MvxViewModelRequest Request
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;
                ViewModel = (IBasketCatalogItemVM)((value as MvxViewModelInstanceRequest).ViewModelInstance);
                DataContext = ViewModel;
            }
        }

        public IBasketCatalogItemVM ViewModel { get; set; }

        protected ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();

        #endregion

        #region Constructor

        public BasketCatalogItemView()
        {
            InitializeControls();
            this.DelayBind(BindControls);
        }

        public BasketCatalogItemView(IntPtr handle)
            : base(handle)
        {

        }

        #endregion

        #region Protected

        #region InitializationControls

        private void InitializeControls()
        {
            Frame = GetFrame();

            var arr = NSBundle.MainBundle.LoadNib(nameof(BasketCatalogItemView), null, null);
            var viewFromNib = Runtime.GetNSObject<BasketCatalogItemView>(arr.ValueAt(0));

            _buyButton = viewFromNib._buyButton;

            SetupBuyButton(_buyButton);

            viewFromNib.Frame = Bounds;
            viewFromNib.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            AddSubview(viewFromNib);

            this.SetupStyle(ThemeConfig.CatalogItem);
        }

        protected virtual CGRect GetFrame()
        {
            return new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width * 0.5f, 40.0f);
        }

        protected virtual void SetupBuyButton(UIButton buyButton)
        {
            buyButton.SetupStyle(ThemeConfig.CatalogItem.BuyButton);
            buyButton.WithTitleForAllStates(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "BasketCatalogCard_AddToBasket"));
        }

        #endregion

        #region BindingControls

        private void BindControls()
        {
            var set = this.CreateBindingSet<BasketCatalogItemView, IBasketCatalogItemVM>();

            BindBuyButton(_buyButton, set);

            set.Apply();
        }

        protected virtual void BindBuyButton(UIButton buyButton, MvxFluentBindingDescriptionSet<BasketCatalogItemView, IBasketCatalogItemVM> set)
        {
            set.Bind(buyButton).To(vm => vm.BuyCommand);
        }

        #endregion

        #endregion
    }
}
