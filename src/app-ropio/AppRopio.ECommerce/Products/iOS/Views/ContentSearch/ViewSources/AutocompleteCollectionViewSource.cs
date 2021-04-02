using System.Runtime.CompilerServices;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Autocomplete;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.Autocomplete;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ContentSearch.ViewSources
{
    public class AutocompleteCollectionViewSource : MvxCollectionViewSource
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public AutocompleteCollectionViewSource(UICollectionView collectionView, NSString defaultIdentifier)
            : base(collectionView, defaultIdentifier)
        {
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var itemVm = GetItemAt(indexPath) as IAutocompleteItemVM;

            var itemSpacing = ((layout as UICollectionViewFlowLayout)?.MinimumInteritemSpacing ?? 0f) * 2f;
            var maxWidth = (DeviceInfo.ScreenWidth - collectionView.ContentInset.Left - collectionView.ContentInset.Right - itemSpacing) / 2.0f;

            if (itemVm != null)
            {
                var label = new AppRopio.Base.iOS.Controls.ARLabel { Text = itemVm.AutocompleteText };

                label.SetupStyle(ThemeConfig.ContentSearch.AutocompeleteCell.Title);
                label.SizeToFit();

                return new CGSize(label.Bounds.Width + AutocompleteCell.HORIZONTAL_MARGINS, AutocompleteCell.CONTENT_HEIGHT);
            }

            return new CGSize(52, collectionView.Bounds.Height);
        }
    }
}
