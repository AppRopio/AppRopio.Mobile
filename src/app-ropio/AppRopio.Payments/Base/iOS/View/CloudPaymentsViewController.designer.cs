// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;
using AppRopio.Base.iOS;

namespace AppRopio.Payments.CloudPayments.iOS.View
{
    [Register ("CardPaymentViewController")]
    partial class CardPaymentViewController
    {
        [Outlet]
        AppRopio.Base.iOS.Controls.ARTextField CardHolderTextField { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARTextField CardNumberTextField { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARTextField CvvTextField { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARTextField ExpirationDateTextField { get; set; }

        [Outlet]
        UIKit.UIButton PayButton { get; set; }

        [Outlet]
        BindableWebView WebView { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (WebView != null) {
                WebView.Dispose ();
                WebView = null;
            }

            if (CardHolderTextField != null) {
                CardHolderTextField.Dispose ();
                CardHolderTextField = null;
            }

            if (CardNumberTextField != null) {
                CardNumberTextField.Dispose ();
                CardNumberTextField = null;
            }

            if (CvvTextField != null) {
                CvvTextField.Dispose ();
                CvvTextField = null;
            }

            if (ExpirationDateTextField != null) {
                ExpirationDateTextField.Dispose ();
                ExpirationDateTextField = null;
            }

            if (PayButton != null) {
                PayButton.Dispose ();
                PayButton = null;
            }
        }
    }
}
