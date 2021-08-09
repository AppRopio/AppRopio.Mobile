using System;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Logging;
using MvvmCross.UI;
using UIKit;

namespace AppRopio.Base.iOS.Binding
{
    public class AnimatedVisibilityBinding : MvxConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        public AnimatedVisibilityBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(MvxVisibility);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIView)target;
            var visibility = (MvxVisibility)value;
            switch (visibility)
            {
                case MvxVisibility.Visible:
                    view.SetHiddenAnimated(false);
                    break;

                case MvxVisibility.Collapsed:
                    view.SetHiddenAnimated(true);
                    break;

                default:
                    Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"Visibility out of range {value}");
                    break;
            }
        }
    }
}
