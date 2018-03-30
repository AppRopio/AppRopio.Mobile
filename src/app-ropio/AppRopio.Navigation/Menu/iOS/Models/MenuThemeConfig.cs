using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppRopio.Navigation.Menu.iOS.Models
{
    public class MenuThemeConfig
    {
        [JsonProperty("navBarMenuImage")]
        public Image NavBarMenuImage { get; private set; }

        [JsonProperty("flyoutController")]
        public FlyoutController FlyoutController { get; private set; }

        [JsonProperty("leftViewController")]
        public LeftViewController LeftViewController { get; private set; }

        public MenuThemeConfig()
        {
            NavBarMenuImage = new Image();
            FlyoutController = new FlyoutController();
            LeftViewController = new LeftViewController();
        }
    }

    public class FlyoutController
    {
        [JsonProperty("menuSlidesWithTopView")]
        public bool MenuSlidesWithTopView { get; private set; }
    }

    public class LeftViewController
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("backgroundColor")]
        public Color BackgroundColor { get; private set; }

        [JsonProperty("menuTable")]
        public MenuTable MenuTable { get; private set; }

        [JsonProperty("logotypeHeaderImage")]
        public Image LogotypeHeaderImage { get; private set; }

        public LeftViewController()
        {
            Size = new Size { Width = 270 };
            BackgroundColor = (Color)Theme.ColorPalette.BackgroundMenu.Clone();
            MenuTable = new MenuTable();
        }
    }

    public class MenuTable
    {
        [JsonProperty("margins")]
        public Margins Margins { get; private set; }

        [JsonProperty("contentInsets")]
        public ContentInsets ContentInsets { get; private set; }

        [JsonProperty("background")]
        public Color Background { get; private set; }

        [JsonProperty("menuCell")]
        public MenuCell MenuCell { get; private set; }

        public List<OverlayCellTheme> OverlayCellThemes { get; private set; }

        [JsonProperty("sectionHeaderHeight")]
        public float SectionHeaderHeight { get; private set; }

        public MenuTable()
        {
            Margins = new Margins { Top = 20 };
            ContentInsets = new ContentInsets { Top = 44 };
            Background = (Color)Theme.ColorPalette.BackgroundMenu.Clone();
            MenuCell = new MenuCell();
            SectionHeaderHeight = 0;
            OverlayCellThemes = new List<OverlayCellTheme>();
        }
    }

    public class OverlayCellTheme
    {
        public IndexPath IndexPath { get; set; }

        public string ViewModelType { get; set; }

        [JsonProperty("menuCell")]
        public MenuCell MenuCell { get; private set; }

        public OverlayCellTheme()
        {
            MenuCell = new MenuCell();
        }
    }

    public class MenuCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("name")]
        public Label Name { get; private set; }

        [JsonProperty("badge")]
        public Label Badge { get; private set; }

        public MenuCell()
        {
            Size = new Size { Height = 50 };
            Name = new Label { TextColor = (Color)Theme.ColorPalette.TextMenu.Clone(), Font = Theme.FontsPalette.SemiboldOfSize(16) };
            Badge = new Label { TextColor = (Color)Theme.ColorPalette.TextAccent.Clone(), Font = Theme.FontsPalette.SemiboldOfSize(14), Background = (Color)Theme.ColorPalette.Accent.Clone(), TextAlignment = TextAlignment.Center };
        }
    }
}
