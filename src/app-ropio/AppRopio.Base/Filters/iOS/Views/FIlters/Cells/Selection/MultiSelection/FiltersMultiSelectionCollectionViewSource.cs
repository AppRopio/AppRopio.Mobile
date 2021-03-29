using System.Runtime.CompilerServices;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection.Items;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.Filters.iOS.Views.FIlters.Cells.Selection.MultiSelection.Cells;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.ViewSources;
using CoreGraphics;
using Foundation;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Selection.MultiSelection
{
    public class FiltersMultiSelectionCollectionViewSource : BaseCollectionViewSource
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public FiltersMultiSelectionCollectionViewSource(UICollectionView collectionView, NSString defaultIdentifier)
            : base(collectionView, defaultIdentifier)
        {
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var itemVm = GetItemAt(indexPath) as MultiCollectionItemVM;

            if (itemVm != null)
            {
                var label = new AppRopio.Base.iOS.Controls.ARLabel { Frame = new CGRect(0, 0, 0, MultiSelectionTextCell.LABEL_HEIGHT), Text = itemVm.ValueName };

                label.SetupStyle(ThemeConfig.Filters.FiltersCell.MultiSelection.MultiSelectionCell.Value);
                label.SizeToFit();

                return new CGSize(label.Bounds.Width + MultiSelectionTextCell.HORIZONTAL_MARGINS, collectionView.Bounds.Height);
            }

            return new CGSize(74, collectionView.Bounds.Height);
        }
    }
}