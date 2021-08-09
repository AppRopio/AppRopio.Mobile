using System;
using MvvmCross;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Logging;
using UIKit;

namespace AppRopio.Base.iOS.Binding
{
    public class UIButtonTitleColorTargetBinding : MvxConvertingTargetBinding
    {
        private readonly UIControlState _state;

        protected UIButton Button => Target as UIButton;

        public UIButtonTitleColorTargetBinding(UIButton button, UIControlState state = UIControlState.Normal)
            : base(button)
        {
            this._state = state;
            if (button == null)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Error("Error - UIButton is null in UIButtonTitleColorTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(UIColor);

        protected override void SetValueImpl(object target, object value)
        {
            ((UIButton)target).SetTitleColor(value as UIColor, this._state);
        }
    }
}
