using System;
using UIKit;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this UIImageView view, Models.ThemeConfigs.Image model)
        {
            if (model?.Path == null)
                return;

            view.Image = model.Mask != null ?
                ImageCache.GetImage(model.Path, model.Mask.ToUIColor()) :
                ImageCache.GetImage(model.Path);

            if (model.HighlightedPath != null)
            {
                view.HighlightedImage = model.Mask != null ?
                    ImageCache.GetImage(model.HighlightedPath, model.Mask.ToUIColor()) :
                    ImageCache.GetImage(model.HighlightedPath);
            }

            if (model.States != null)
            {
                if (model.States.Normal != null && model.States.Normal.Content != null)
                {
                    view.Image = ImageCache.GetImage(model.Path, model.States.Normal.Content.ToUIColor());
                }

                if (model.States.Highlighted != null && model.States.Highlighted.Content != null)
                {
                    view.HighlightedImage = ImageCache.GetImage(model.HighlightedPath ?? model.Path, model.States.Highlighted.Content.ToUIColor());
                }
            }
        }
    }
}
