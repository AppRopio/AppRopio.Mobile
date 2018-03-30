using System;

using UIKit;
using AppRopio.Base.iOS;

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
        }
    }
}

