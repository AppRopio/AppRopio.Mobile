using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace AppRopio.Base.Droid.Controls
{
    [Register("appropio.base.droid.controls.ARAspectLayout")]
    public class ARAspectLayout : FrameLayout
    {
        //TODO: change Ratio dynamically if called from code
        public float Aspect { get; set; } = 1.0f;

        public ARAspectLayout(Context context) :
            base(context)
        {
        }

        public ARAspectLayout(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            ParseAttrs(attrs);
        }

        public ARAspectLayout(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            ParseAttrs(attrs);
        }

        public ARAspectLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            ParseAttrs(attrs);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = (int)(width * Aspect);
            heightMeasureSpec = MeasureSpec.MakeMeasureSpec(height, Android.Views.MeasureSpecMode.Exactly);
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        private void ParseAttrs(IAttributeSet attrs) {
            var styleAttrs = Context.ObtainStyledAttributes(attrs, Resource.Styleable.ARAspectLayout);
            if (styleAttrs != null) {
                Aspect = styleAttrs.GetFloat(Resource.Styleable.ARAspectLayout_aspect, Aspect);
            }
        }
    }
}
