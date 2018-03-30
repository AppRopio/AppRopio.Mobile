// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;

namespace AppRopio.ECommerce.Marked.iOS.Views.Marked
{
    [Register("MarkedListCell")]
    partial class MarkedListCell
    {
        [Outlet]
        UIKit.UIImageView _image { get; set; }

        [Outlet]
        UIKit.UIButton _markButton { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _oldPrice { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _price { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (_name != null)
            {
                _name.Dispose();
                _name = null;
            }

            if (_oldPrice != null)
            {
                _oldPrice.Dispose();
                _oldPrice = null;
            }

            if (_price != null)
            {
                _price.Dispose();
                _price = null;
            }

            if (_markButton != null)
            {
                _markButton.Dispose();
                _markButton = null;
            }

            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }
        }
    }
}
