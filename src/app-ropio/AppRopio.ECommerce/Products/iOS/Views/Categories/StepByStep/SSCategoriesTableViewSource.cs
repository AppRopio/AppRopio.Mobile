using System;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep
{
    public class SSCategoriesTableViewSource : MvxSimpleTableViewSource
    {
        protected virtual ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public SSCategoriesTableViewSource(UITableView tableView, NSString key1, NSString key2)
            : base (tableView, key1, key2)
        {
            
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            nfloat height = 0;

            var item = GetItemAt(indexPath) as ICategoriesItemVM;

            if (!item?.BackgroundImageUrl.IsNullOrEmpty() ?? false)
                height = DeviceInfo.ScreenWidth * 9 / 16;
            else
                height = (nfloat)ThemeConfig.Categories.CategoryCell.Size.Height;

            return height;
        }
    }
}