// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal
{
    [Register("HorizontalCollectionCell")]
    partial class HorizontalCollectionCell
    {
        [Outlet]
        UIKit.UIView _bottonSeparator { get; set; }

        [Outlet]
        UIKit.UICollectionView _collectionView { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (_collectionView != null)
            {
                _collectionView.Dispose();
                _collectionView = null;
            }

            if (_name != null)
            {
                _name.Dispose();
                _name = null;
            }

            if (_bottonSeparator != null)
            {
                _bottonSeparator.Dispose();
                _bottonSeparator = null;
            }
        }
    }
}
