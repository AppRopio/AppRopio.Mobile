using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request;
using Java.Net;
using MvvmCross.Platform.Platform;
using Base64Decoder = Android.Util.Base64;

namespace AppRopio.Base.Droid.Controls
{
    [Register("appropio.base.droid.controls.ARImageView")]
    public class ARImageView : ImageView
    {
        enum DownScaleType
        {
            None,
            FixSize
        }

        public bool Circle { get; set; }

        private DownScaleType downScaleType = DownScaleType.None;

        private int downScale;
        private Drawable placeholder;

        public ARImageView(Context context) :
            base(context)
        {
            
        }

        public ARImageView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            ParseAttrs(attrs);
        }

        public ARImageView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            ParseAttrs(attrs);
        }

        private void ParseAttrs(IAttributeSet attrs)
        {
            var styleAttrs = Context.ObtainStyledAttributes(attrs, Resource.Styleable.ARImageView);
            if (styleAttrs != null)
            {
                downScale = styleAttrs.GetDimensionPixelSize(Resource.Styleable.ARImageView_downscale, 0);
                if (downScale != 0)
                    downScaleType = DownScaleType.FixSize;
                else
                {
                    downScaleType = DownScaleType.None;
                }
                Circle = styleAttrs.GetBoolean(Resource.Styleable.ARImageView_circle, false);
                placeholder = styleAttrs.GetDrawable(Resource.Styleable.ARImageView_placeholder);
            }
        }

        public override void SetImageBitmap(Android.Graphics.Bitmap bm)
        {
            base.SetImageBitmap(bm);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;

                GlideClear();

                if (value.IsNullOrEmtpy())
                {
                    if (placeholder != null)
                        this.SetImageDrawable(placeholder);
                }
                else
                {
                    var options = new RequestOptions();

                    if (IsResourceUrl(value))
                    {
                        var resId = GetResource(value);

                        if (resId > 0)
                            Glide
                              .With(this.Context)
                              .Load(resId)
                              .Apply(GetRequestOptions().DontAnimate())
                              .Into(this);
                    }
                    else
                    {
                        try
                        {
                            Glide
                               .With(this.Context)
                                .Load(new URL(value))
                                .Apply(GetRequestOptions())
                                .Into(this);
                        }
                        catch (Exception ex)
                        {
                            MvxTrace.Trace(() => $"[ARImageView] Error while loading url: {value}; Message {ex.BuildAllMessagesAndStackTrace()}");
                        }
                    }
                }
            }
        }

        private void GlideClear()
        {
            var activity = this.Context as Activity;
            if (activity != null && !activity.IsFinishing && !activity.IsDestroyed)
                Glide.With(this.Context).Clear(this);
        }

        private string _base64;
        public string Base64
        {
            get
            {
                return _base64;
            }
            set
            {
                _base64 = value;
                if (value.IsNullOrEmtpy())
                {
                    if (placeholder != null)
                        this.SetImageDrawable(placeholder);
                }
                else
                {
                    Task.Run(() =>
                    {
                        var imageArray = Base64Decoder.Decode(value, Android.Util.Base64Flags.Default);

                        Application.SynchronizationContext.Post(_ =>
                        {
                            var activity = this.Context as Activity;
                            if (activity != null && !activity.IsFinishing && !activity.IsDestroyed)
                                try
                                {
                                    Glide
                                        .With(this.Context)
                                        .AsBitmap()
                                        .Load(imageArray)
                                        .Apply
                                        (
                                            GetRequestOptions()
                                            .DontAnimate()
                                        )
                                            .Into(this);
                                }
                                catch (Exception ex)
                                {
                                    MvxTrace.Trace(() => $"[ARImageView] Error while loading base64 image; Message {ex.BuildAllMessagesAndStackTrace()}");
                                }
                        }, null);
                    });
                }
            }
        }

        private int _resourceId;
        public int ResourceId
        {
            get
            {
                return _resourceId;
            }
            set
            {
                _resourceId = value;
                if (value > 0)

                    Glide
                        .With(this.Context)
                        .Load(value)
                        .Apply(GetRequestOptions().DontAnimate())
                        .Into(this);
            }
        }

        private int GetResource(string url)
        {
            try
            {
                string resName = string.Empty;
                var indexOfRes = url.IndexOf("res:", StringComparison.InvariantCultureIgnoreCase);
                if (indexOfRes >= 0)
                {
                    resName = url.Remove(indexOfRes, 4);
                }
                if (!resName.IsNullOrEmtpy())
                {
                    resName.Trim(' ');
                }
                else
                    return -1;
                return Resources.GetIdentifier(resName, "drawable", this.Context.PackageName);
            }
            catch (Exception ex)
            {
                MvxTrace.Trace(() => $"[ARImageView] Fail to get ResourceId from string:{url}");

                return -1;
            }
        }

        private bool IsResourceUrl(string url)
        {
            return url.StartsWith("res:", StringComparison.InvariantCultureIgnoreCase);
        }

        private RequestOptions GetRequestOptions()
        {
            var options = new RequestOptions();

            if (placeholder != null)
                options = options.Placeholder(placeholder);

            if (downScaleType == DownScaleType.FixSize)
                options = options.Override(downScale);

            if (Circle)
                options = options.CircleCrop();

            return options;
        }
    }
}
