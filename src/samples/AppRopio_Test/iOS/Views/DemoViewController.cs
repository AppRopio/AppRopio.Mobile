using System;

using UIKit;
using AppRopio.Base.iOS;
using AppRopio.Base.Core;
using AppRopio.Base.iOS.UIExtentions;
using System.Globalization;

namespace AppRopio_Test.Views
{
    public partial class DemoViewController : UIViewController
    {
        public Action Callback { get; }

        public DemoViewController(Action callback)
            : base("DemoViewController", null)
        {
            Callback = callback;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            _agreementButton.TouchUpInside -= OnButtonClick;
            Callback();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _agreementButton.SetupStyle(Theme.ControlPalette.Button.Base);
            _agreementButton.TouchUpInside += OnButtonClick;
            _agreementButton.Layer.CornerRadius = 24;

            if (CultureInfo.CurrentUICulture.Parent.Name != "ru-RU")
            {
                _agreementTitle.Text = "Welcome";
                _agreementText.Text = "The application was developed on the AppRopio platform. \n\nAll data is fictitious.\n\nDo not be scared if something went wrong.";
                _agreementButton.WithTitleForAllStates("OK, LET'S START");
            }
        }
    }
}

