using System;
using AppRopio.Base.Information.iOS.Models;
using AppRopio.Base.Information.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Models.Information.Responses;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Information.iOS.Views.Cell
{
    public partial class ArticleCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ArticleCell");
        public static readonly UINib Nib;

		protected InformationThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IInformationThemeConfigService>().ThemeConfig; } }

		static ArticleCell()
        {
            Nib = UINib.FromName("ArticleCell", NSBundle.MainBundle);
        }

        protected ArticleCell(IntPtr handle) : base(handle)
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
            SetupTitle(_title);

            this.SetupStyle(ThemeConfig.InformationCell);
        }

		protected virtual void SetupTitle(UILabel title)
		{
            title.SetupStyle(ThemeConfig.InformationCell.Title);
		}

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var bindingSet = this.CreateBindingSet<ArticleCell, Article>();

            BindTitle(_title, bindingSet);

            bindingSet.Apply();
        }

		protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<ArticleCell, Article> set)
		{
            set.Bind(title).To(vm => vm.Title);
		}

        #endregion
    }
}
