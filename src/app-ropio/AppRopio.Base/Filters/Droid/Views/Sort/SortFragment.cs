using System;
using Android.Animation;
using Android.Graphics;
using AppRopio.Base.Droid.Views;
using AppRopio.Base.Filters.Core.ViewModels.Sort;

namespace AppRopio.Base.Filters.Droid.Views.Sort
{
    public class SortFragment : CommonFragment<ISortViewModel>
    {
        public SortFragment()
            : base(Resource.Layout.app_filters_sort)
        {
        }

        public override void OnResume()
        {
            base.OnResume();

            var backgroundAnimator = ValueAnimator.OfArgb(Color.Transparent.ToArgb(), new Color(0, 0, 0, 126).ToArgb());
            backgroundAnimator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
            {
                View.SetBackgroundColor(new Color((int)e.Animation.AnimatedValue));
            };
            backgroundAnimator.StartDelay = 300;
            backgroundAnimator.SetDuration(300);
            backgroundAnimator.Start();
        }

        public override void OnPause()
        {
            View.SetBackgroundColor(Color.Transparent);

            base.OnPause();
        }
    }
}
