using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks;
using AppRopio.ECommerce.Basket.iOS.Models;
using AppRopio.ECommerce.Basket.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Thanks
{
    public partial class ThanksOrderViewController : CommonViewController<IThanksViewModel>
    {
		protected BasketThemeConfig Theme { get { return Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig; } }

		public ThanksOrderViewController() : base("ThanksOrderViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            SetupImage(_image);
            SetupTitle(TitleLabel);
            SetupOrderNumber(OrderNumberLabel);
            SetupContinueTitle(ContinueLabel);
            SetupCatalogButton(CatalogButton);
        }

        protected virtual void SetupImage(UIImageView image)
        {
            image.SetupStyle(Theme.Thanks.Image);
        }

        protected virtual void SetupTitle(UILabel title)
		{
            title.SetupStyle(Theme.Thanks.Title);
            title.Text = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Thanks_Title");
		}

		protected virtual void SetupOrderNumber(UILabel orderNumber)
		{
            orderNumber.SetupStyle(Theme.Thanks.OrderNumber);
		}

		protected virtual void SetupContinueTitle(UILabel continueTitle)
		{
            continueTitle.SetupStyle(Theme.Thanks.OrderNumber);
            continueTitle.Text = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Thanks_Text");
		}

        protected virtual void SetupCatalogButton(UIButton catalogButton)
		{
            catalogButton.SetupStyle(Theme.Thanks.CatalogButton);
            catalogButton.WithTitleForAllStates(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Thanks_Catalog"));
		}

		protected override void BindControls()
		{
			var set = this.CreateBindingSet<ThanksOrderViewController, IThanksViewModel>();

            BindCatalogButton(CatalogButton, set);
            BindCloseButton(CloseButton, set);
            BindOrderNumber(OrderNumberLabel, set);

			set.Apply();
		}

		#endregion

        protected virtual void BindCatalogButton(UIButton catalogButton, MvxFluentBindingDescriptionSet<ThanksOrderViewController, IThanksViewModel> set)
		{
            set.Bind(catalogButton).To(vm => vm.GoToCatalogCommand);
		}

		protected virtual void BindCloseButton(UIButton closeButton, MvxFluentBindingDescriptionSet<ThanksOrderViewController, IThanksViewModel> set)
		{
            set.Bind(closeButton).To(vm => vm.CloseCommand);
		}

        protected virtual void BindOrderNumber(UILabel orderNumber, MvxFluentBindingDescriptionSet<ThanksOrderViewController, IThanksViewModel> set)
		{
            set.Bind(orderNumber).To(vm => vm.OrderId).WithConversion("StringFormat", "№{0}");
		}
    }
}