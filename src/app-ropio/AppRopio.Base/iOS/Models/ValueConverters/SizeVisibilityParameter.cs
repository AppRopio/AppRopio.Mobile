using System;
using UIKit;
namespace AppRopio.Base.iOS.Models.ValueConverters
{
    public class SizeVisibilityParameter
    {
        public UIView View { get; set; }

        public Func<nfloat> MaximumHeight { get; set; }
        public Func<nfloat> MinimumHeight { get; set; }

        public Func<nfloat> MaximumWidth { get; set; }
        public Func<nfloat> MinimumWidth { get; set; }
    }
}
