using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using UIKit;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this UIBarButtonItem view, Models.ThemeConfigs.Button model)
        {
            if (model.Image != null && model.Image.Path != null)
            {
                view.Image = ImageCache.GetImage(model.Image.Path);

                if (model.ImageInsets != null)
                    view.ImageInsets = (UIEdgeInsets)model.ImageInsets;
            }

            if (model.TextColor != null && model.Font != null)
                view.SetTitleTextAttributes(new UITextAttributes { Font = (UIFont)model.Font, TextColor = (UIColor)model.TextColor }, UIControlState.Normal);

            if (model.States != null)
            {
                if (model.States.Normal != null)
                {
                    if (model.States.Normal.Content != null)
                        view.SetTitleTextAttributes(new UITextAttributes { Font = (UIFont)model.Font, TextColor = (UIColor)model.States.Normal.Content }, UIControlState.Normal);
                }

                if (model.States.Highlighted != null)
                {
                    if (model.States.Highlighted.Content != null)
                    {
                        view.SetTitleTextAttributes(new UITextAttributes { Font = (UIFont)model.Font, TextColor = (UIColor)model.States.Highlighted.Content }, UIControlState.Highlighted);
                        view.TintColor = (UIColor)model.States.Highlighted.Content;
                    }
                }

                if (model.States.Selected != null)
                {
                    if (model.States.Selected.Content != null)
                        view.SetTitleTextAttributes(new UITextAttributes { Font = (UIFont)model.Font, TextColor = (UIColor)model.States.Selected.Content }, UIControlState.Selected);
                }

                if (model.States.Disabled != null)
                {
                    if (model.States.Disabled.Content != null)
                        view.SetTitleTextAttributes(new UITextAttributes { Font = (UIFont)model.Font, TextColor = (UIColor)model.States.Disabled.Content }, UIControlState.Disabled);
                }
            }
        }

        public static void SetupStyle(this UIButton view, Models.ThemeConfigs.Button model)
        {
            if (model.UppercaseTitle)
            {
                var normalTitle = view.Title(UIControlState.Normal);
                if (!string.IsNullOrEmpty(normalTitle))
                    view.SetTitle(normalTitle.ToUpperInvariant(), UIControlState.Normal);

                var highlightedTitle = view.Title(UIControlState.Highlighted);
                if (!string.IsNullOrEmpty(highlightedTitle))
                    view.SetTitle(highlightedTitle.ToUpperInvariant(), UIControlState.Highlighted);

                var disabledTitle = view.Title(UIControlState.Disabled);
                if (!string.IsNullOrEmpty(disabledTitle))
                    view.SetTitle(disabledTitle.ToUpperInvariant(), UIControlState.Disabled);
            }

            if (model.TextColor != null)
                view.SetTitleColor(model.TextColor.ToUIColor(), UIControlState.Normal);

            if (model.Font != null)
                view.Font = (UIFont)model.Font;

            if (model.Background != null)
                view.BackgroundColor = model.Background.ToUIColor();

            if (model.TitleInsets != null)
                view.TitleEdgeInsets = (UIEdgeInsets)model.TitleInsets;

            if (model.Image != null && model.Image.Path != null)
            {
                var states = model.Image.States ?? model.States;

                if (states != null)
                {
                    if (states.Normal != null && states.Normal.Content != null)
                        view.SetImage(ImageCache.GetImage(model.Image.Path, states.Normal.Content.ToUIColor()), UIControlState.Normal);

                    if (states.Highlighted != null && states.Highlighted.Content != null)
                        view.SetImage(ImageCache.GetImage(model.Image.HighlightedPath ?? model.Image.Path, states.Highlighted.Content.ToUIColor()), UIControlState.Highlighted);

                    if (states.Selected != null && states.Selected.Content != null)
                        view.SetImage(ImageCache.GetImage(model.Image.HighlightedPath ?? model.Image.Path, states.Selected.Content.ToUIColor()), UIControlState.Selected);

                    if (states.Disabled != null && states.Disabled.Content != null)
                        view.SetImage(ImageCache.GetImage(model.Image.Path, states.Disabled.Content.ToUIColor()), UIControlState.Disabled);
                }
                else
                {
                    view.SetImage(ImageCache.GetImage(model.Image.Path), UIControlState.Normal);
                    view.SetImage(ImageCache.GetImage(model.Image.HighlightedPath ?? model.Image.Path), UIControlState.Highlighted);
                    view.SetImage(ImageCache.GetImage(model.Image.HighlightedPath ?? model.Image.Path), UIControlState.Selected);
                }

                if (model.ImageInsets != null)
                    view.ImageEdgeInsets = (UIEdgeInsets)model.ImageInsets;
            }

            if (model.States != null)
            {
                if (model.States.Normal != null)
                {
                    if (model.States.Normal.Content != null)
                        view.SetTitleColor(model.States.Normal.Content.ToUIColor(), UIControlState.Normal);
                    if (model.States.Normal.Background != null)
                        view.SetBackgroundImage(model.States.Normal.Background
                                                .ToUIColor()
                                                .ToUIImageRounded(/*model.Layer?.CornerRadius*/), UIControlState.Normal);
                }

                if (model.States.Highlighted != null)
                {
                    if (model.States.Highlighted.Content != null)
                        view.SetTitleColor(model.States.Highlighted.Content.ToUIColor(), UIControlState.Highlighted);
                    if (model.States.Highlighted.Background != null)
                        view.SetBackgroundImage(model.States.Highlighted.Background
                                                .ToUIColor()
                                                .ToUIImageRounded(/*model.Layer?.CornerRadius*/), UIControlState.Highlighted);
                }

                if (model.States.Selected != null)
                {
                    if (model.States.Selected.Content != null)
                        view.SetTitleColor(model.States.Selected.Content.ToUIColor(), UIControlState.Selected);
                    if (model.States.Selected.Background != null)
                        view.SetBackgroundImage(model.States.Selected.Background
                                                .ToUIColor()
                                                .ToUIImageRounded(/*model.Layer?.CornerRadius*/), UIControlState.Selected);
                }

                if (model.States.Disabled != null)
                {
                    if (model.States.Disabled.Content != null)
                        view.SetTitleColor(model.States.Disabled.Content.ToUIColor(), UIControlState.Disabled);
                    if (model.States.Disabled.Background != null)
                        view.SetBackgroundImage(model.States.Disabled.Background
                                                .ToUIColor()
                                                .ToUIImageRounded(/*model.Layer?.CornerRadius*/), UIControlState.Disabled);
                }
            }
            else
            {
                view.SetTitleColor(model.TextColor.ToUIColor(), UIControlState.Normal);
                view.BackgroundColor = model.Background.ToUIColor();
            }

            if (model.Layer != null)
                view.Layer.SetupStyle(model.Layer);
        }
    }
}
