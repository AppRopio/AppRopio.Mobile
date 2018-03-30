// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Selection.Cells
{
    [Register("SelectionCell")]
    partial class SelectionCell
    {
        [Outlet]
        UIKit.UIView _bottomSeparator { get; set; }

        [Outlet]
        UIKit.UIImageView _selectionImageView { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _valueName { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (_valueName != null)
            {
                _valueName.Dispose();
                _valueName = null;
            }

            if (_bottomSeparator != null)
            {
                _bottomSeparator.Dispose();
                _bottomSeparator = null;
            }

            if (_selectionImageView != null)
            {
                _selectionImageView.Dispose();
                _selectionImageView = null;
            }
        }
    }
}
