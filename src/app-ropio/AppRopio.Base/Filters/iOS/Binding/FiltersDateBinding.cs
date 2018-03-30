using System;
using System.Globalization;
using System.Text;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;
using System.Text.RegularExpressions;

namespace AppRopio.Base.Filters.iOS.Binding
{
    public class FiltersDateBinding : MvxTargetBinding
    {
        private string _filter;

        protected UITextField TextField
        {
            get { return (UITextField)Target; }
        }

        public FiltersDateBinding(UITextField target)
            : base(target)
        {
            _filter = $"##{CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator}##{CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator}####";
        }

        public override void SubscribeToEvents()
        {
            TextField.Text = "";
            TextField.ShouldChangeCharacters = ShouldChangePhoneNumber;
        }

        public override void SetValue(object value)
        {
            if (value is DateTimeOffset)
            {
                var formatedValue = FilteredPhoneString(
                    ((DateTimeOffset)value).ToString(
                        CultureInfo.CurrentCulture == new CultureInfo("ru-RU") ?
                            "dd.MM.yyyy" :
                            $"MM{CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator}dd{CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator}yyyy"
                    ),
                    _filter
                );
                TextField.Text = formatedValue;
            }
        }

        public override Type TargetType
        {
            get { return typeof(string); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = Target as UITextField;
                if (target != null)
                {
                    target.Dispose();
                }
            }
            base.Dispose(isDisposing);
        }

        private bool ShouldChangePhoneNumber(UITextField textField, NSRange range, string replacementString)
        {
            var changedString = (range.Location > 0 ? textField.Text.Substring(0, (int)range.Location) : "")
                + replacementString
                + (textField.Text.Substring(
                    (int)(range.Location + range.Length),
                    (int)(textField.Text.Length - range.Location - range.Length)));

            if (range.Length == 1 && replacementString.Length == 0 &&
                !char.IsDigit(textField.Text[(int)range.Location]))
            {
                // Something was deleted.  Delete past the previous number
                int location = changedString.Length - 1;
                if (location > 0)
                {
                    for (; location > 0; location--)
                    {
                        if (char.IsDigit(changedString[location]))
                            break;
                    }
                    changedString = changedString.Substring(0, location);
                }
            }

            textField.Text = FilteredPhoneString(changedString, _filter);

            DateTimeOffset dateTime;
            if (textField.Text.Length == _filter.Length && DateTimeOffset.TryParse(textField.Text, out dateTime))
                FireValueChanged(dateTime);

            return false;
        }

        private string FilteredPhoneString(string str, string filter)
        {
            int onOriginal = 0, onFilter = 0, onOutput = 0;
            var outputString = new StringBuilder();
            bool done = false;

            while (onFilter < filter.Length && !done)
            {
                var filterChar = filter[onFilter];
                var originalChar = onOriginal >= str.Length ? (char)0 : str[onOriginal];
                switch (filterChar)
                {
                    case '#':
                        if (originalChar == 0)
                        {
                            done = true;
                            break;
                        }
                        if (char.IsDigit(originalChar))
                        {
                            outputString.Append(originalChar);
                            onOriginal++;
                            onFilter++;
                            onOutput++;
                        }
                        else
                        {
                            onOriginal++;
                        }
                        break;
                    default:
                        // Any other character will automatically be inserted for the user as they type (spaces, - etc..) or deleted as they delete if there are more numbers to come.
                        outputString.Append(filterChar);
                        onOutput++;
                        onFilter++;
                        if (originalChar == filterChar)
                            onOriginal++;
                        break;
                }
            }
            return outputString.ToString();
        }
    }
}
