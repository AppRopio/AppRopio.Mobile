// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Vertical.Cells
{
    [Register("VerticalTextCell")]
    partial class VerticalTextCell
    {
        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

        [Outlet]
        UIKit.UIView _selectedView { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (_name != null)
            {
                _name.Dispose();
                _name = null;
            }

            if (_selectedView != null)
            {
                _selectedView.Dispose();
                _selectedView = null;
            }
        }
    }
}
