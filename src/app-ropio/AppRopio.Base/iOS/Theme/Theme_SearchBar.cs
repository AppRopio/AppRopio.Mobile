using System;
using Foundation;
using UIKit;
using AppRopio.Base.iOS.Helpers;
using System.Linq;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this BindableSearchBar view, Models.ThemeConfigs.SearchBar model)
        {
            view.IsShowCancelOnEditing = true;

            SetupStyle((UISearchBar)view, model);
        }

        public static void SetupStyle(this UISearchBar view, Models.ThemeConfigs.SearchBar model)
        {
            if (view is BindableSearchBar)
                ((BindableSearchBar)view).IsShowCancelOnEditing = true;
            
            view.BackgroundColor = (UIColor)model.BackgroundColor;
            view.BarTintColor = (UIColor)model.BackgroundColor;
            view.TintColor = (UIColor)model.TextField.TextColor;

            if (model.SearchImage != null)
                view.SetImageforSearchBarIcon(ImageCache.GetImage(model.SearchImage.Path, (UIColor)model.TextField.Placeholder.TextColor), UISearchBarIcon.Search, UIControlState.Normal);

            view.Layer.BorderWidth = 0f;
            view.Layer.BorderColor = view.BackgroundColor.CGColor;

            view.BackgroundImage = model.BackgroundColor.ToUIImage();
            view.SetBackgroundImage(
                model.BackgroundColor.ToUIImage(),
                UIBarPosition.Any,
                UIBarMetrics.Default
            );

            var searchTextField = view.ValueForKey(new NSString("_searchField")) as UITextField;
            if (searchTextField != null)
            {
                searchTextField.BackgroundColor = (UIColor)model.TextField.Background;
                searchTextField.Font = (UIFont)model.TextField.Font;
                searchTextField.TextColor = (UIColor)model.TextField.TextColor;
                searchTextField.TintColor = (UIColor)model.TextField.TintColor;
                searchTextField.AttributedPlaceholder = new NSMutableAttributedString("Поиск", (UIFont)model.TextField.Placeholder.Font, (UIColor)model.TextField.Placeholder.TextColor);
            }
        }
    }
}
