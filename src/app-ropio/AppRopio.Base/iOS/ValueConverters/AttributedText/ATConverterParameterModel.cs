using System;
using Foundation;
using UIKit;

namespace AppRopio.Base.iOS.ValueConverters
{
    public class ATConverterParameterModel
    {
        public NSUnderlineStyle UnderlineStyle { get; set; }

        public NSUnderlineStyle StrikethroughStyle { get; set; }

        public Func<object, string> StringFormat { get; set; }

        public float Kerning { get; set; }

        public bool Uppercase { get; set; }

        public UIFont Font { get; set; }

        public UIColor ForegroundColor { get; set; }

        public bool IncludedCurrency { get; set; }
    }
}