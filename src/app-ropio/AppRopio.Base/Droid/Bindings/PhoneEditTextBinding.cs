using System;
using System.Linq;
using System.Text;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace AppRopio.Base.Droid.Bindings
{
    public class PhoneEditTextBinding : MvxAndroidTargetBinding
    {
        private string _prevText;

        protected EditText EditText
        {
            get { return (EditText)Target; }
        }

        public PhoneEditTextBinding(EditText target)
            : base(target)
        {
        }

        public override void SubscribeToEvents()
        {
            _prevText = "+7 (";
            EditText.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            var target = Target as EditText;

            if (target == null)
                return;

            var value = GetFilteredPhoneString(target.Text);

            //            if (!value.Replace("+7 (", "").IsNullOrEmtpy())
            {
                EditText.TextChanged -= OnTextChanged;
                EditText.Text = value;
                EditText.SetSelection(EditText.Text.Length);
                EditText.TextChanged += OnTextChanged;

                FireValueChanged(value);
            }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var editText = (EditText)target;
            editText.Text = ((string)value);
            editText.SetSelection(editText.Text.Length);
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
                var target = Target as EditText;
                if (target != null)
                {
                    target.TextChanged -= OnTextChanged;
                }
            }
            base.Dispose(isDisposing);
        }

        private string GetFilteredPhoneString(string text)
        {
            var filter = @"+ 7 (###) ### ## ##";
            string filteredPhoneString = "";
            var prevText = _prevText.Replace(" ", "");

            if (text.Replace(" ", "").Length < 3)
            {
                filteredPhoneString = "+ 7 (";
            }
            else
            {
                if (_prevText.Length > text.Length)
                {
                    if (EndsOnNumeric(_prevText))
                    {
                        filteredPhoneString = FilteredPhoneString(text, filter);
                    }
                    else if (prevText.Last() == ')' || prevText.Last() == '-')
                    {
                        filteredPhoneString = FilteredPhoneString(prevText.Substring(0, prevText.Length - 2), filter);
                    }
                }
                else
                    filteredPhoneString = FilteredPhoneString(text, filter);//.Replace('-', ' ');
            }

            _prevText = filteredPhoneString;
            return filteredPhoneString;
        }

        private bool EndsOnNumeric(string s)
        {
            return s.Replace(" ", "").Last().IsNumeric();
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
                            if (outputString.Length > 1 && outputString[outputString.Length - 1] == ' ')
                                outputString.Remove(outputString.Length - 1, 1);
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
