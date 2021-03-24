using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Items;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Cells
{
    public partial class PDHorizontalColorCell : MvxCollectionViewCell
    {
        private UIImageView _imageView;

        public static readonly NSString Key = new NSString("PDHorizontalColorCell");
        public static readonly UINib Nib;

        static PDHorizontalColorCell()
        {
            Nib = UINib.FromName("PDHorizontalColorCell", NSBundle.MainBundle);
        }

        protected PDHorizontalColorCell(IntPtr handle)
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
            var set = this.CreateBindingSet<PDHorizontalColorCell, CollectionItemVM>();

            BindColorView(_colorView, set);
            BindSelectedView(_selectedView, set);
            BindImageView(_imageView, set);

            set.Apply();
        }

        protected virtual void BindColorView(UIView colorView, MvxFluentBindingDescriptionSet<PDHorizontalColorCell, CollectionItemVM> set)
        {
            set.Bind(colorView).For(v => v.BackgroundColor).To(vm => vm.Value).WithConversion("Color");
        }

        protected virtual void BindSelectedView(UIView selectedView, MvxFluentBindingDescriptionSet<PDHorizontalColorCell, CollectionItemVM> set)
        {
            set.Bind(selectedView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
        }

        protected virtual void BindImageView(UIImageView imageView, MvxFluentBindingDescriptionSet<PDHorizontalColorCell, CollectionItemVM> set)
        {
            set.Bind(imageView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
        }

        #endregion

        #endregion
    }
}
