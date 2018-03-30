using System;
using System.Text;
using AppRopio.Base.iOS.Helpers;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace AppRopio.Base.iOS.Binding
{
    public class PhoneBinding : MvxTargetBinding
	{
        private PhoneFormatter _phoneFormatter;

        protected UITextField TextField
		{
			get { return (UITextField)Target; }
		}

		public PhoneBinding(UITextField target)
			: base(target)
		{
            _phoneFormatter = new PhoneFormatter() { FireValueChanged = this.FireValueChanged };
		}

		public override void SubscribeToEvents()
		{
            TextField.Text = PhoneFormatter.INITIAL_STRING;
            TextField.ShouldChangeCharacters = _phoneFormatter.ShouldChangePhoneNumber;
		}

		public override void SetValue(object value)
		{
			var str = value as string;
			if (string.IsNullOrEmpty(str))
				return;

            var formatedValue = _phoneFormatter.FilteredPhoneString(str);
			TextField.Text = formatedValue;
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
	}
}
