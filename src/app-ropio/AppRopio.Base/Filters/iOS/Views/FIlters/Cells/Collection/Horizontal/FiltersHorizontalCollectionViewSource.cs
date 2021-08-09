using System.Runtime.CompilerServices;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal.Cells;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.ViewSources;
using CoreGraphics;
using Foundation;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal
{
    public class FiltersHorizontalCollectionViewSource : BaseCollectionViewSource
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public FiltersHorizontalCollectionViewSource(UICollectionView collectionView)
            : base(collectionView)
        {

        }

        protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, Foundation.NSIndexPath indexPath, object item)
        {
            var itemVm = item as CollectionItemVM;

            if (itemVm != null)
            {
                switch (itemVm.DataType)
                {
                    case AppRopio.Models.Filters.Responses.FilterDataType.Color:
                        return (UICollectionViewCell)collectionView.DequeueReusableCell(HorizontalColorCell.Key, indexPath);
                    default:
                        return (UICollectionViewCell)collectionView.DequeueReusableCell(HorizontalTextCell.Key, indexPath);
                }
            }

            return null;
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var itemVm = GetItemAt(indexPath) as CollectionItemVM;

            if (itemVm != null)
            {
                if (itemVm.DataType == AppRopio.Models.Filters.Responses.FilterDataType.Text)
                {
                    var label = new AppRopio.Base.iOS.Controls.ARLabel { Text = itemVm.ValueName };

                    label.SetupStyle(ThemeConfig.Filters.FiltersCell.Collection.Value);
                    label.SizeToFit();

                    return new CGSize(label.Bounds.Width + HorizontalTextCell.HORIZONTAL_MARGINS, collectionView.Bounds.Height);
                }
            }

            return new CGSize(40, collectionView.Bounds.Height);
        }
    }
}