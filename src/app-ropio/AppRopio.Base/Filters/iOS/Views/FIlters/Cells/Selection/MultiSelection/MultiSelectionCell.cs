using System;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.Filters.iOS.Views.FIlters.Cells.Selection.MultiSelection.Cells;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Selection.MultiSelection
{
    public partial class MultiSelectionCell : MvxTableViewCell
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public const float MULTY_SELECTION_TITLE_HEIGHT = 52;
        public const float MULTY_SELECTION_CONTENT_HEIGHT = 30;

        public static readonly NSString Key = new NSString("MultiSelectionCell");
        public static readonly UINib Nib;

        static MultiSelectionCell()
        {
            Nib = UINib.FromName("MultiSelectionCell", NSBundle.MainBundle);
        }

        protected MultiSelectionCell(IntPtr handle)
            : base(handle)
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
            SetupName(_name);

            SetupCollectionView(_collectionView);

            _accessoryImageView.Image = ImageCache.GetImage("Images/Main/accessory_arrow.png");

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.Title);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            var flowLayout = collectionView.CollectionViewLayout as UICollectionViewFlowLayout;

            flowLayout.MinimumInteritemSpacing = 8;
            
            collectionView.ContentInset = new UIEdgeInsets(0, 16, 0, 16);
            collectionView.ShowsHorizontalScrollIndicator = false;

            collectionView.RegisterNibForCell(MultiSelectionTextCell.Nib, MultiSelectionTextCell.Key);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<MultiSelectionCell, IMultiSelectionFiVm>();

            BindName(_name, set);
            BindCollectionView(_collectionView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<MultiSelectionCell, IMultiSelectionFiVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<MultiSelectionCell, IMultiSelectionFiVm> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);
            collectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            set.Bind(collectionView).For("Visibility").To(vm => vm.Items.Count).WithConversion("Visibility");
        }

        protected virtual MvxCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new FiltersMultiSelectionCollectionViewSource(collectionView, MultiSelectionTextCell.Key);
        }

        #endregion
    }
}
