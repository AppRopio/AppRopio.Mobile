using System;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Vertical;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Vertical.Cells;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Vertical
{
    public partial class VerticalCollectionCell : MvxTableViewCell
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public const float VERTICAL_COLLECTION_TITLE_HEIGHT = 44;
        public const float VERTICAL_COLLECTION_ITEM_HEIGHT = 44;
        public const float VERTICAL_COLLECTION_BOTTOM_INSET = 15;

        public static readonly NSString Key = new NSString("VerticalCollectionCell");
        public static readonly UINib Nib;

        static VerticalCollectionCell()
        {
            Nib = UINib.FromName("VerticalCollectionCell", NSBundle.MainBundle);
        }

        protected VerticalCollectionCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region Protected

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupName(_name);
            SetupCollectionView(_collectionView);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.Title);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            var flowLayout = collectionView.CollectionViewLayout as UICollectionViewFlowLayout;
            flowLayout.SectionInset = new UIEdgeInsets(0, 8, 0, 8);
            flowLayout.MinimumLineSpacing = 6;
            flowLayout.MinimumInteritemSpacing = 6;

            flowLayout.ItemSize = new CoreGraphics.CGSize((DeviceInfo.ScreenWidth - flowLayout.SectionInset.Left - flowLayout.SectionInset.Right - (flowLayout.MinimumInteritemSpacing * 4)) / 4, collectionView.Bounds.Height);

            collectionView.RegisterNibForCell(VerticalTextCell.Nib, VerticalTextCell.Key);

            //_bottomCollectionViewConstraint.Constant = VERTICAL_COLLECTION_BOTTOM_INSET;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<VerticalCollectionCell, IVerticalCollectionFiVm>();

            BindName(_name, set);
            BindCollectionView(_collectionView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<VerticalCollectionCell, IVerticalCollectionFiVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<VerticalCollectionCell, IVerticalCollectionFiVm> set)
        {
            var dataSource = SetupCollectionViewDataSource(collectionView);

            collectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            collectionView.ReloadData();
        }

        protected virtual MvxCollectionViewSource SetupCollectionViewDataSource(UICollectionView collectionView)
        {
            return new MvxCollectionViewSource(collectionView, VerticalTextCell.Key);
        }

        #endregion

        #endregion
    }
}
