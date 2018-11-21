//
//  Copyright 2018  
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Linq;
using System.Text;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;

namespace AppRopio.Base.Droid.Bindings
{
    public class UpdatedTextBinding : MvxAndroidTargetBinding
    {
        protected EditText EditText
        {
            get { return (EditText)Target; }
        }

        public UpdatedTextBinding(EditText target)
            : base(target)
        {
        }

        public override void SubscribeToEvents()
        {
            EditText.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            if (EditText == null)
                return;

            var value = EditText.Text;

            EditText.TextChanged -= OnTextChanged;

            EditText.Text = value;
            EditText.SetSelection(EditText.Text.Length);

            EditText.TextChanged += OnTextChanged;

            FireValueChanged(value);
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
    }
}
