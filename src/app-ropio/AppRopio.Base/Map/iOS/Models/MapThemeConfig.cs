using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using Newtonsoft.Json;

namespace AppRopio.Base.Map.iOS.Models
{
    public class MapThemeConfig
    {
        [JsonProperty("points")]
        public Points Points { get; private set; }

        public MapThemeConfig()
        {
            Points = new Points();
        }
    }

    public class Points
    {
        [JsonProperty("pageTitle")]
        public Label PageTitle { get; private set; }

        [JsonProperty("basePointInfo")]
        public PointInfo BasePointInfo { get; private set; }

        [JsonProperty("list")]
        public PointsList List { get; private set; }

        [JsonProperty("map")]
        public PointsMap Map { get; private set; }

        [JsonProperty("additionalInfo")]
        public AdditionalInfo AdditionalInfo { get; private set; }

        public Points()
        {
            PageTitle = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                HighlightedTextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            BasePointInfo = new PointInfo();
            List = new PointsList();
            Map = new PointsMap();
            AdditionalInfo = new AdditionalInfo();
        }
    }

    public class PointsList
    {
        [JsonProperty("searchBar")]
        public SearchBar SearchBar { get; private set; }

        [JsonProperty("cell")]
        public PointCell Cell { get; private set; }

        public Image MapImage { get; private set; }

        public PointsList()
        {
            SearchBar = (SearchBar)Theme.ControlPalette.SearchBar.Clone();
            Cell = new PointCell();
            MapImage = new Image();
        }
    }

    public class PointsMap
    {
        [JsonProperty("pinImage")]
        public Image PinImage { get; private set; }

        [JsonProperty("info")]
        public PointInfo Info { get; private set; }
    }

    public class AdditionalInfo
    {
        [JsonProperty("closeButton")]
        public Button CloseButton { get; private set; }

        [JsonProperty("info")]
        public PointInfo Info { get; private set; }

        public AdditionalInfo()
        {
            CloseButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
        }
    }

    public class PointCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("checkImage")]
        public Image CheckImage { get; private set; }

        [JsonProperty("info")]
        public PointInfo Info { get; private set; }

        public PointCell()
        {
            Size = new Size { Width = DeviceInfo.ScreenWidth, Height = 200 };
        }
    }

    public class PointInfo : View
    {
        [JsonProperty("titleLabel")]
        public Label TitleLabel { get; private set; }

        [JsonProperty("addressLabel")]
        public Label AddressLabel { get; private set; }

        [JsonProperty("workTimeLabel")]
        public Label WorkTimeLabel { get; private set; }

        [JsonProperty("distanceLabel")]
        public Label DistanceLabel { get; private set; }

        [JsonProperty("distanceImage")]
        public Image DistanceImage { get; private set; }

        [JsonProperty("infoLabel")]
        public Label InfoLabel { get; private set; }

        [JsonProperty("callButton")]
        public Button CallButton { get; private set; }

        [JsonProperty("infoButton")]
        public Button InfoButton { get; private set; }

        [JsonProperty("routeButton")]
        public Button RouteButton { get; private set; }

        public PointInfo()
        {
            TitleLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            AddressLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(13)
            };
            WorkTimeLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(13)
            };
            DistanceLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(13)
            };
            InfoLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(13)
            };
            CallButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            InfoButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            RouteButton = (Button)Theme.ControlPalette.Button.Base.Clone();
        }
    }
}
