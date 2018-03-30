using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Hint;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.Hint
{
    public partial class HintCell : MvxTableViewCell
    {
        public const float CONTENT_HEIGHT = 52;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("HintCell");
        public static readonly UINib Nib;

        static HintCell()
        {
            Nib = UINib.FromName("HintCell", NSBundle.MainBundle);
        }

        protected HintCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region Protected

        #region InitializaitonControls

        protected virtual void InitializeControls()
        {
            SetupTitle(_title);
            SetupLinkImage(_linkImageView);

            this.SetupStyle(ThemeConfig.ContentSearch.SearchCell);
        }

        protected virtual void SetupTitle(UILabel title)
        {
            title.SetupStyle(ThemeConfig.ContentSearch.SearchCell.Title);
        }

        protected virtual void SetupLinkImage(UIImageView linkImageView)
        {
            linkImageView.SetupStyle(ThemeConfig.ContentSearch.SearchCell.LinkIcon);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<HintCell, IHintItemVM>();

            BindTitle(_title, set);

            set.Apply();
        }

        protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<HintCell, IHintItemVM> set)
        {
            set.Bind(title).To(vm => vm.SearchText);
        }

        #endregion

        #endregion
    }
}
