using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep.Cells
{
    public partial class SSCategoryCell : MvxTableViewCell
    {
        private CAGradientLayer _gradientLayer;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("SSCategoryCell");
        public static readonly UINib Nib = UINib.FromName("SSCategoryCell", NSBundle.MainBundle);

        protected SSCategoryCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            var cell = ThemeConfig.Categories.CategoryCell;

            _stackViewHeight.Constant = cell.Size.Height ?? 0;

            SetupBackgroundImage(_backgroundImage, cell.BackgroundImage);

            SetupIcon(_icon, cell.Icon);

            SetupName(_name, cell.Title);

            this.SetupStyle(ThemeConfig.Categories.CategoryCell);

            SetupGradientView(_gradientView);
        }

        protected virtual void SetupGradientView(UIView gradientView)
        {
            var height = DeviceInfo.ScreenWidth * 9 / 16;

            _gradientLayer = new CAGradientLayer()
            {
                Colors = new CGColor[] { UIColor.FromRGBA(39, 31, 31, 96).CGColor, UIColor.FromWhiteAlpha(0.0f, 0.0f).CGColor },
                Locations = new NSNumber[] { NSNumber.FromFloat(0.0f), NSNumber.FromFloat(1.0f) },
                StartPoint = new CGPoint(0.0f, 1.0f),
                EndPoint = new CGPoint(0.0f, 0.0f),
                Frame = new CGRect(0, 0, gradientView.Bounds.Width, gradientView.Bounds.Height)
            };

            gradientView.Layer.AddSublayer(_gradientLayer);
        }

        protected virtual void SetupBackgroundImage(UIImageView image, Image imageModel)
        {
            image?.SetupStyle(imageModel);
        }

        protected virtual void SetupIcon(UIImageView image, Image imageModel)
        {
            image?.SetupStyle(imageModel);
        }

        protected virtual void SetupName(UILabel name, Label label)
        {
            name.SetupStyle(label);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<SSCategoryCell, ICategoriesItemVM>();

            BindBackgroundImage(_backgroundImage, set);
            BindIcon(_icon, set);
            BindName(_name, set);
            BindGradientView(_gradientView, set);

            set.Apply();
        }

        protected virtual void BindGradientView(UIView gradientView, MvxFluentBindingDescriptionSet<SSCategoryCell, ICategoriesItemVM> set)
        {
            set.Bind(gradientView).For("Visibility").To(vm => vm.BackgroundImageUrl).WithConversion("Visibility");
        }

        protected virtual void BindBackgroundImage(UIImageView backgroundImage, MvxFluentBindingDescriptionSet<SSCategoryCell, ICategoriesItemVM> set)
        {
            if (backgroundImage == null)
                return;

            var imageLoader = new MvxImageViewLoader(() => backgroundImage)
            {
                DefaultImagePath = $"res:{ThemeConfig.Categories.CategoryCell.BackgroundImage.Path}",
                ErrorImagePath = $"res:{ThemeConfig.Categories.CategoryCell.BackgroundImage.Path}"
            };

            set.Bind(imageLoader).To(vm => vm.BackgroundImageUrl);
            set.Bind(backgroundImage).For("Visibility").To(vm => vm.BackgroundImageUrl).WithConversion("Visibility");
        }

        protected virtual void BindIcon(UIImageView image, MvxFluentBindingDescriptionSet<SSCategoryCell, ICategoriesItemVM> set)
        {
            if (image == null)
                return;

            var imageLoader = new MvxImageViewLoader(() => image)
            {
                DefaultImagePath = $"res:{ThemeConfig.Categories.CategoryCell.Icon.Path}",
                ErrorImagePath = $"res:{ThemeConfig.Categories.CategoryCell.Icon.Path}"
            };

            set.Bind(imageLoader).To(vm => vm.IconUrl);
            set.Bind(image).For("Visibility").To(vm => vm.IconUrl).WithConversion("Visibility");
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<SSCategoryCell, ICategoriesItemVM> set)
        {
            if (name == null)
                return;

            set.Bind(name).To(vm => vm.Name);
        }

        #endregion

        #region Public

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            _name.Highlighted = true;

            base.TouchesBegan(touches, evt);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            _name.Highlighted = false;

            base.TouchesCancelled(touches, evt);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            _name.Highlighted = false;

            base.TouchesEnded(touches, evt);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_gradientView != null && _gradientLayer != null)
                _gradientLayer.Frame = new CGRect(0, 0, _gradientView.Bounds.Width, _gradientView.Bounds.Height);
        }

        #endregion
    }
}
