using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
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

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep.Cells
{
    public partial class CategoryGridCell : MvxCollectionViewCell
    {
        private CAGradientLayer _gradientLayer;

        protected virtual ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("CategoryGridCell");
        public static readonly UINib Nib;

        static CategoryGridCell()
        {
            Nib = UINib.FromName("CategoryGridCell", NSBundle.MainBundle);
        }

        protected CategoryGridCell(IntPtr handle) : base(handle)
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

            SetupBackgroundImage(_backgroundImage);

            SetupImage(_image, cell.Icon);

            SetupName(_name, cell.Title);

            this.SetupStyle(cell);

            SetupGradientView(_gradientView);

            _separator.BackgroundColor = cell.Title.TextColor.ToUIColor();
        }

        protected virtual void SetupGradientView(UIView gradientView)
        {
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

        protected virtual void SetupBackgroundImage(UIImageView image)
        {
            
        }

        protected virtual void SetupImage(UIImageView image, Image imageModel)
        {
            image?.SetupStyle(imageModel);
        }

        protected virtual void SetupName(UILabel name, Label nameLabel)
        {
            name?.SetupStyle(nameLabel);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<CategoryGridCell, ICategoriesItemVM>();

            BindBackgroundImage(_backgroundImage, set);
            BindImage(_image, set);
            BindName(_name, set);
            BindGradientView(_gradientView, set);

            set.Apply();
        }

        protected virtual void BindGradientView(UIView gradientView, MvxFluentBindingDescriptionSet<CategoryGridCell, ICategoriesItemVM> set)
        {
            set.Bind(gradientView).For("Visibility").To(vm => vm.BackgroundImageUrl).WithConversion("Visibility");
        }

        protected virtual void BindBackgroundImage(UIImageView backgroundImage, MvxFluentBindingDescriptionSet<CategoryGridCell, ICategoriesItemVM> set)
        {
            if (backgroundImage == null)
                return;

            var imageLoader = new MvxImageViewLoader(() => backgroundImage);

            set.Bind(imageLoader).To(vm => vm.BackgroundImageUrl);
        }

        protected virtual void BindImage(UIImageView image, MvxFluentBindingDescriptionSet<CategoryGridCell, ICategoriesItemVM> set)
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

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<CategoryGridCell, ICategoriesItemVM> set)
        {
            if (name == null) 
                return;

            set.Bind(name).To(vm => vm.Name);
        }

        #endregion

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_gradientView != null && _gradientLayer != null)
                _gradientLayer.Frame = new CGRect(0, 0, _gradientView.Bounds.Width, _gradientView.Bounds.Height);
        }
    }
}
