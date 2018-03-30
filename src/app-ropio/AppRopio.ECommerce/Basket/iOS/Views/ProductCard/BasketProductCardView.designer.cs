// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;

namespace AppRopio.ECommerce.Basket.iOS.Views.ProductCard
{
	[Register("BasketProductCardView")]
	partial class BasketProductCardView
    {
        [Outlet]
        UIKit.UIButton _buyButton { get; set; }

        [Outlet]
        UIKit.UIButton _decrementBtn { get; set; }

        [Outlet]
        UIKit.UIButton _incrementBtn { get; set; }

        [Outlet]
        UIKit.UITextField _quantityTextField { get; set; }

        [Outlet]
        UIKit.UIView _stepperView { get; set; }

		void ReleaseDesignerOutlets()
		{
            if (_buyButton != null)
            {
                _buyButton.Dispose();
                _buyButton = null;
            }
            if (_stepperView != null)
            {
                _stepperView.Dispose();
                _stepperView = null;
            }

            if (_quantityTextField != null)
            {
                _quantityTextField.Dispose();
                _quantityTextField = null;
            }

            if (_decrementBtn != null)
            {
                _decrementBtn.Dispose();
                _decrementBtn = null;
            }

            if (_incrementBtn != null)
            {
                _incrementBtn.Dispose();
                _incrementBtn = null;
            }
		}
	}
}
