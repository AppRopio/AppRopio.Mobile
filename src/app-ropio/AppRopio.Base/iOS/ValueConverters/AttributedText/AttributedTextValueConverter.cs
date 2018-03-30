using System;
using MvvmCross.Platform.Converters;
using Foundation;
using System.Globalization;
using UIKit;

namespace AppRopio.Base.iOS.ValueConverters
{
    public class AttributedTextValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var asConverterParameter = parameter as ATConverterParameterModel;

            if (asConverterParameter != null)
            {
                var text = string.Format(asConverterParameter.StringFormat?.Invoke(value) ?? "{0}", value).TrimStart().TrimEnd();

                if (asConverterParameter.Uppercase)
                    text = text.ToUpperInvariant();

                var attrString = new NSMutableAttributedString(str: text,
                                              font: asConverterParameter.Font,
                                              foregroundColor: asConverterParameter.ForegroundColor,
                                              strokeColor: asConverterParameter.ForegroundColor,
                                              underlineStyle: asConverterParameter.UnderlineStyle,
                                              strikethroughStyle: asConverterParameter.StrikethroughStyle,
                                              kerning: asConverterParameter.Kerning);

                if (asConverterParameter.IncludedCurrency)
                {
                    attrString.AddAttribute(UIStringAttributeKey.Font, UIFont.SystemFontOfSize(asConverterParameter.Font.PointSize), new NSRange(text.IndexOf('\u20BD'), 1));
                }

                return attrString;
            }

            return null;
        }
    }
}