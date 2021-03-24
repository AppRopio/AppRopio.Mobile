using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.History;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.History
{
    public partial class HistoryCell : MvxTableViewCell
    {
        public const float CONTENT_HEIGHT = 52;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("HistoryCell");
        public static readonly UINib Nib;

        static HistoryCell()
        {
            Nib = UINib.FromName("HistoryCell", NSBundle.MainBundle);
        }

        protected HistoryCell(IntPtr handle)
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
            SetupHistoryImageView(_historyImageView);
            SetupTitle(_title);
            SetupLinkImage(_linkImageView);

            this.SetupStyle(ThemeConfig.ContentSearch.SearchCell);
        }

        protected virtual void SetupHistoryImageView(UIImageView historyImageView)
        {
            historyImageView.SetupStyle(ThemeConfig.ContentSearch.SearchCell.HistoryIcon);
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
            var set = this.CreateBindingSet<HistoryCell, IHistorySearchItemVM>();

            BindTitle(_title, set);

            set.Apply();
        }

        protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<HistoryCell, IHistorySearchItemVM> set)
        {
            set.Bind(title).To(vm => vm.SearchText);
        }

        #endregion

        #endregion
    }
}
