using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace AppRopio.Base.Droid.Adapters {
	public class ARAppCompatSpinnerAdapter : MvxAdapter
    {
        private bool dropDownWidthCalculated = false;

        public Action<float> SetDropDownWidth { get; set; }

        public ARAppCompatSpinnerAdapter(Context context)
            : this(context, MvxAndroidBindingContextHelpers.Current())
        {
        }

        public ARAppCompatSpinnerAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
            
        }

        protected ARAppCompatSpinnerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private void SetDropdownWidth()
        {
            if ((ItemsSource?.Count() ?? 0) < 1)
            {
                SetDropDownWidth?.Invoke(TypedValue.ApplyDimension(ComplexUnitType.Dip, 150, this.Context.Resources.DisplayMetrics));
            }
            else
            {
                int maxWidth = 0;

                var paint = new Paint();

                paint.TextSize = Context.Resources.GetDimension(Resource.Dimension.app_b1_text_size);

                paint.SetTypeface(Typeface.Default);

                paint.SetStyle(Paint.Style.Fill);

                Rect result = new Rect();

                foreach (var item in ItemsSource)
                {
                    var value = item?.ToString() ?? "";
                    paint.GetTextBounds(value, 0, value.Length, result);
                    if (maxWidth < result.Width()) //тк не факт, что самая длинная строка будет самой широкой
                        maxWidth = result.Width();
                }

                var margins = (TypedValue.ApplyDimension(ComplexUnitType.Dip, 16, this.Context.Resources.DisplayMetrics)) * 2;
                var error = (TypedValue.ApplyDimension(ComplexUnitType.Dip, 8, this.Context.Resources.DisplayMetrics)); //иначе на некоторых девайсах не влезает
                var targetWidth = error + maxWidth + margins;
                var maxScreenWidth = this.Context.Resources.DisplayMetrics.WidthPixels - margins;

                SetDropDownWidth?.Invoke(targetWidth > maxScreenWidth ? maxScreenWidth : targetWidth);

                dropDownWidthCalculated = true;
            }
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            return GetBindableView(convertView, BindingContext.DataContext, parent, ItemTemplateId);
        }

        public override Android.Views.View GetDropDownView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            return base.GetDropDownView(position, convertView, parent);
        }

        protected override void RealNotifyDataSetChanged()
        {
            SetDropdownWidth();
            base.RealNotifyDataSetChanged();
        }
    }
}
