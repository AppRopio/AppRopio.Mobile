using System;
using System.Text;
using Foundation;
using UIKit;

namespace AppRopio.Base.iOS.Helpers
{
    public class PhoneFormatter
    {
        public const string FILTER = @"+ 7 (###) ### ## ##";

        public const string INITIAL_STRING = @"+ 7 (";

        public Action<string> FireValueChanged { get; set; }

        public bool ShouldChangePhoneNumber(UITextField textField, NSRange range, string replacementString)
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

            textField.Text = FilteredPhoneString(changedString);

            FireValueChanged?.Invoke(textField.Text);

            return false;
        }

        public string FilteredPhoneString(string str)
        {
            int onOriginal = 0, onFilter = 0, onOutput = 0;
            var outputString = new StringBuilder();
            bool done = false;

            while (onFilter < FILTER.Length && !done)
            {
                var filterChar = FILTER[onFilter];
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
