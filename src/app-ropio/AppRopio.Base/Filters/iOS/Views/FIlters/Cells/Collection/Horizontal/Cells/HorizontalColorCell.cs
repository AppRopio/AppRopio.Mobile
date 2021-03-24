using System;

using Foundation;
using MvvmCross.Platforms.Ios.Binding;
using UIKit;
using MvvmCross.Binding.BindingContext;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Helpers;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal.Cells
{
    public partial class HorizontalColorCell : MvxCollectionViewCell
    {
        private UIImageView _imageView;

        public static readonly NSString Key = new NSString("HorizontalColorCell");
        public static readonly UINib Nib;

        static HorizontalColorCell()
        {
            Nib = UINib.FromName("HorizontalColorCell", NSBundle.MainBundle);
        }

        protected HorizontalColorCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region Protected

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupColorView(_colorView);
            SetupSelectedView(_selectedView);
            SetupImageView(_imageView = new UIImageView());
        }

        protected virtual void SetupColorView(UIView colorView)
        {
            colorView.Layer.CornerRadius = colorView.Bounds.Width / 2;
        }

        protected virtual void SetupSelectedView(UIView selectedView)
        {
            selectedView.BackgroundColor = UIColor.Clear;
            selectedView.Layer.BorderWidth = 1;
            selectedView.Layer.BorderColor = Theme.ColorPalette.Accent.ToUIColor().CGColor;
            selectedView.Layer.CornerRadius = selectedView.Bounds.Width / 2;
        }

        protected virtual void SetupImageView(UIImageView imageView)
        {
            imageView.Image = ImageCache.GetImage("Images/Filters/Choice.png");
            imageView.TranslatesAutoresizingMaskIntoConstraints = false;

            this.AddSubview(imageView);

            this.AddConstraints(new NSLayoutConstraint[]
            {
                NSLayoutConstraint.Create(imageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(imageView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0)
            });
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<HorizontalColorCell, CollectionItemVM>();

            BindColorView(_colorView, set);
            BindSelectedView(_selectedView, set);
            BindImageView(_imageView, set);

            set.Apply();
        }

        protected virtual void BindColorView(UIView colorView, MvxFluentBindingDescriptionSet<HorizontalColorCell, CollectionItemVM> set)
        {
            set.Bind(colorView).For(v => v.BackgroundColor).To(vm => vm.Value).WithConversion("Color");
        }

        protected virtual void BindSelectedView(UIView selectedView, MvxFluentBindingDescriptionSet<HorizontalColorCell, CollectionItemVM> set)
        {
            set.Bind(selectedView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
        }

        protected virtual void BindImageView(UIImageView imageView, MvxFluentBindingDescriptionSet<HorizontalColorCell, CollectionItemVM> set)
        {
            set.Bind(imageView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
        }

        #endregion

        #endregion
    }
}
