using System;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged.Cells
{
    public partial class CCategoryCell : MvxCollectionViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("CCategoryCell");
        public static readonly UINib Nib = UINib.FromName("CCategoryCell", NSBundle.MainBundle);

        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;

                if (_name != null)
                    _name.Highlighted = value;
            }
        }

        public CCategoryCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        protected virtual void InitializeControls()
        {
            SetupName(_name);
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Categories.TabCell.Title);
        }

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<CCategoryCell, ICategoriesItemVM>();
            set.Bind(_name).To(vm => vm.Name);
            set.Apply();
        }
    }
}
