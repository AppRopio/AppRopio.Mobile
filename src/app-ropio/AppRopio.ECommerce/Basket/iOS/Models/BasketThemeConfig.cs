using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;
using AppRopio.Base.iOS.UIExtentions;
using System.Collections.Generic;

namespace AppRopio.ECommerce.Basket.iOS.Models
{
    public class BasketThemeConfig
    {
        [JsonProperty("cartIndicator")]
        public BasketCartIndicator CartIndicator { get; private set; }

        [JsonProperty("productCard")]
        public BasketProductCard ProductCard { get; private set; }

        [JsonProperty("basket")]
        public Basket Basket { get; private set; }

        [JsonProperty("order")]
        public Order Order { get; private set; }

        [JsonProperty("thanks")]
        public Thanks Thanks { get; private set; }

        public BasketThemeConfig()
        {
            CartIndicator = new BasketCartIndicator();
            ProductCard = new BasketProductCard();
            Basket = new Basket();
            Order = new Order();
            Thanks = new Thanks();
        }
    }

    public class BasketCartIndicator
    {
        public Label Badge { get; private set; }

        public Image Image { get; private set; }

        public BasketCartIndicator()
        {
            Badge = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextAccent.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(12),
                Background = (Color)Theme.ColorPalette.Accent.Clone(),
                TextAlignment = TextAlignment.Center,
                Layer = new Layer
                {
                    MaskToBounds = true,
                    CornerRadius = 8
                }
            };

            Image = new Image();
        }
    }

    public class BasketProductCard : View
    {
        [JsonProperty("buyButton")]
        public Button BuyButton { get; private set; }

        [JsonProperty("quantityButton")]
        public Button QuantityButton { get; private set; }

        [JsonProperty("quantityField")]
        public TextField QuantityField { get; private set; }

        [JsonProperty("unitName")]
        public Label UnitName { get; private set; }

        [JsonProperty("incrementButton")]
        public Button IncrementButton { get; private set; }

        [JsonProperty("decrementButton")]
        public Button DecrementButton { get; private set; }

        public BasketProductCard()
        {
            BuyButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            QuantityButton = (Button)Theme.ControlPalette.Button.Accessory.Clone();

            QuantityField = new TextField
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14),
                Placeholder = new Label
                {
                    Font = Theme.FontsPalette.BoldOfSize(14),
                    TextColor = (Color)Theme.ColorPalette.Placeholder.Clone()
                },
                Layer = new Layer
                {
                    Border = new Border { Color = (Color)Theme.ColorPalette.Separator.Clone(), Width = 2 },
                    MaskToBounds = true,
                    CornerRadius = 22
                }
            };
            UnitName = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };

            IncrementButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
            DecrementButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
        }
    }

    public class Basket
    {
        [JsonProperty("cell")]
        public BasketCell Cell { get; private set; }

        [JsonProperty("nextButton")]
        public Button NextButton { get; private set; }

        [JsonProperty("emptyView")]
        public BasketEmptyView EmptyView { get; private set; }

        public View BottomView { get; private set; }

        public Basket()
        {
            Cell = new BasketCell();
            NextButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            EmptyView = new BasketEmptyView();
            BottomView = new View();
        }
    }

    public class BasketCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("nameLabel")]
        public Label NameLabel { get; private set; }

        [JsonProperty("priceLabel")]
        public Label PriceLabel { get; private set; }

        [JsonProperty("oldPriceLabel")]
        public Label OldPriceLabel { get; private set; }

        [JsonProperty("quantityLabel")]
        public Label QuantityLabel { get; private set; }

        [JsonProperty("incButton")]
        public Button IncButton { get; private set; }

        [JsonProperty("decButton")]
        public Button DecButton { get; private set; }

        public BasketCell()
        {
            Size = new Size { Width = DeviceInfo.ScreenWidth, Height = 120 };
            NameLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };

            PriceLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };

            OldPriceLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };

            QuantityLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14),
                TextAlignment = TextAlignment.Center
            };

            IncButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
            DecButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
        }
    }

    public class BasketEmptyView : View
    {
        [JsonProperty("image")]
        public Image Image { get; private set; }

        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("text")]
        public Label Text { get; private set; }

        [JsonProperty("catalogButton")]
        public Button CatalogButton { get; private set; }

        public BasketEmptyView()
        {
            Background = (Color)Theme.ColorPalette.Background.Clone();
            Image = new Image();
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16),
                TextAlignment = TextAlignment.Center
            };
            Text = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(16),
                TextAlignment = TextAlignment.Center
            };
            CatalogButton = (Button)Theme.ControlPalette.Button.Base.Clone();
        }
    }

    public class Order
    {
        [JsonProperty("baseOrderFieldCell")]
        public OrderFieldCell BaseOrderFieldCell { get; private set; }

        [JsonProperty("userInfo")]
        public UserInfo UserInfo { get; private set; }

        [JsonProperty("deliveryInfo")]
        public DeliveryInfo DeliveryInfo { get; private set; }

        [JsonProperty("payments")]
        public Payments Payments { get; private set; }

        [JsonProperty("headerLabel")]
        public Label HeaderLabel { get; private set; }

        [JsonProperty("deliveryPriceLabel")]
        public Label DeliveryPriceLabel { get; private set; }

        [JsonProperty("nextButton")]
        public Button NextButton { get; private set; }

        [JsonProperty("accessoryNextButton")]
        public Button AccessoryNextButton { get; private set; }

        public View BottomView { get; private set; }

        [JsonProperty("autocompleteCell")]
        public AutocompleteCell AutocompleteCell { get; private set; }

        public Order()
        {
            BaseOrderFieldCell = new OrderFieldCell();
            UserInfo = new UserInfo();
            DeliveryInfo = new DeliveryInfo();
            Payments = new Payments();
            HeaderLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            DeliveryPriceLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14),
                TextAlignment = TextAlignment.Right
            };
            NextButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            AccessoryNextButton = (Button)Theme.ControlPalette.Button.Accessory.Clone();
            BottomView = new View();
            AutocompleteCell = new AutocompleteCell();
        }
    }

    public class Thanks
    {
        public Image Image { get; private set; }

        public Label Title { get; private set; }

        public Label OrderNumber { get; private set; }

        public Button CatalogButton { get; private set; }

        public Thanks()
        {
            Image = new Image();

            CatalogButton = (Button)Theme.ControlPalette.Button.Base.Clone();

            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16),
                TextAlignment = TextAlignment.Center
            };

            OrderNumber = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(16),
                TextAlignment = TextAlignment.Center
            };
        }
    }

    public class UserInfo
    {
        [JsonProperty("cell")]
        public OrderFieldCell Cell { get; private set; }

        [JsonProperty("nextButton")]
        public Button NextButton { get; private set; }

        public UserInfo()
        {
            Cell = null;
            NextButton = (Button)Theme.ControlPalette.Button.Base.Clone();
        }
    }

    public class DeliveryInfo
    {
        [JsonProperty("cell")]
        public DeliveryTypeCell Cell { get; private set; }

        [JsonProperty("onAddress")]
        public DeliveryOnAddress OnAddress { get; private set; }

        [JsonProperty("onPoint")]
        public DeliveryOnPoint OnPoint { get; private set; }

        [JsonProperty("timeView")]
        public DeliveryTimeView TimeView { get; private set; }

        [JsonProperty("timeCancelButton")]
        public Button TimeCancelButton { get; private set; }

        [JsonProperty("timeApplyButton")]
        public Button TimeApplyButton { get; private set; }

        [JsonProperty("nextButton")]
        public Button NextButton { get; private set; }

        public DeliveryInfo()
        {
            Cell = new DeliveryTypeCell();

            OnAddress = new DeliveryOnAddress();
            OnPoint = new DeliveryOnPoint();

            TimeCancelButton = (Button)Theme.ControlPalette.Button.Text.Clone();
            TimeCancelButton.Font = Theme.FontsPalette.SemiboldOfSize(14);

            TimeApplyButton = (Button)TimeCancelButton.Clone();
            TimeApplyButton.States = new States
            {
                Normal = new State { Content = (Color)Theme.ColorPalette.Accent.Clone() },
                Highlighted = new State { Content = (Color)Theme.ColorPalette.HighlightedControl.Clone() }
            };

            NextButton = (Button)Theme.ControlPalette.Button.Base.Clone();

            TimeView = new DeliveryTimeView();
        }
    }

    public class DeliveryTimeView : View
    {
        [JsonProperty("timeLabel")]
        public Label TimeLabel { get; private set; }

        [JsonProperty("timeIcon")]
        public Image TimeIcon { get; private set; }

        public DelivetyTimeCell TimeCell { get; private set; }

        public DeliveryTimeView()
        {
            TimeLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16)
            };

            TimeCell = new DelivetyTimeCell();
        }
    }

    public class DeliveryOnAddress
    {
        [JsonProperty("cell")]
        public OrderFieldCell Cell { get; private set; }

        [JsonProperty("deliveryPriceLabel")]
        public Label DeliveryPriceLabel { get; private set; }

        [JsonProperty("closeButton")]
        public Button CloseButton { get; private set; }

        [JsonProperty("nextButton")]
        public Button NextButton { get; private set; }

        [JsonProperty("accessoryNextButton")]
        public Button AccessoryNextButton { get; private set; }

        public View BottomView { get; private set; }

        public DeliveryOnAddress()
        {
            Cell = null;
            DeliveryPriceLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
            };
            CloseButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
            NextButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            AccessoryNextButton = (Button)Theme.ControlPalette.Button.Accessory.Clone();
            BottomView = new View();
        }
    }

    public class DeliveryOnPoint
    {
        [JsonProperty("baseDeliveryPointInfo")]
        public DeliveryPointInfo BaseDeliveryPointInfo { get; private set; }

        [JsonProperty("list")]
        public DeliveryOnPointList List { get; private set; }

        [JsonProperty("map")]
        public DeliveryOnPointMap Map { get; private set; }

        public DeliveryOnPoint()
        {
            BaseDeliveryPointInfo = new DeliveryPointInfo();
            List = new DeliveryOnPointList();
            Map = new DeliveryOnPointMap();
        }
    }

    public class DeliveryOnPointList
    {
        [JsonProperty("closeButton")]
        public Button CloseButton { get; private set; }

        [JsonProperty("searchBar")]
        public SearchBar SearchBar { get; private set; }

        [JsonProperty("cell")]
        public DeliveryPointCell Cell { get; private set; }

        [JsonProperty("nextButton")]
        public Button NextButton { get; private set; }

        public DeliveryOnPointList()
        {
            CloseButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
            SearchBar = (SearchBar)Theme.ControlPalette.SearchBar.Clone();
            Cell = new DeliveryPointCell();
            NextButton = (Button)Theme.ControlPalette.Button.Base.Clone();
        }
    }

    public class DeliveryOnPointMap
    {
        [JsonProperty("closeButton")]
        public Button CloseButton { get; private set; }

        [JsonProperty("pinImage")]
        public Image PinImage { get; private set; }

        [JsonProperty("info")]
        public DeliveryPointInfo Info { get; private set; }

        public DeliveryOnPointMap()
        {
            CloseButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
            Info = null;
        }
    }

    public class OrderFieldCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("field")]
        public TextField Field { get; private set; }

        [JsonProperty("typeImages")]
        public Dictionary<string, Image> TypeImages { get; private set; }

        [JsonProperty("typeActionButtons")]
        public Dictionary<string, Button> TypeActionButtons { get; private set; }

        [JsonProperty("optionalTitleLabel")]
        public Label OptionalTitleLabel { get; private set; }

        [JsonProperty("optionalSwitch")]
        public Switch OptionalSwitch { get; private set; }

        [JsonProperty("optionalTextView")]
        public TextView OptionalTextView { get; private set; }

        [JsonProperty("optionalTextViewSize")]
        public Size OptionalTextViewSize { get; private set; }

        public Label CounterTitle { get; private set; }

        public Label CounterValue { get; private set; }

        public OrderCounterCell CounterPickerCell { get; private set; }

        public OrderFieldCell()
        {
            Size = new Size { Width = DeviceInfo.ScreenWidth, Height = 82 };
            Field = (TextField)Theme.ControlPalette.TextField.Clone();

            OptionalTitleLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            OptionalSwitch = (Switch)Theme.ControlPalette.Switch.Clone();
            OptionalTextView = new TextView
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                TintColor = (Color)Theme.ColorPalette.Accent.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14),
                Background = (Color)Theme.ColorPalette.BackgroundSearch.Clone()
            };
            OptionalTextViewSize = new Size { Width = DeviceInfo.ScreenWidth, Height = 90 };

            CounterTitle = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            CounterValue = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                HighlightedTextColor = (Color)Theme.ColorPalette.Accent.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14),
                TextAlignment = TextAlignment.Right
            };
            CounterPickerCell = new OrderCounterCell();
        }
    }

    public class AutocompleteCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("title")]
        public Label Title { get; private set; }

        public AutocompleteCell()
        {
            Size = new Size { Height = 44 };
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                TextAlignment = TextAlignment.Left,
                Font = Theme.FontsPalette.RegularOfSize(16),
            };
        }
    }

    public class OrderCounterCell
    {
        public Label Value { get; private set; }

        public Size Size { get; private set; }

        public OrderCounterCell()
        {
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16),
                TextAlignment = TextAlignment.Center
            };
            Size = new Size { Height = 31 };
        }
    }

    public class DeliveryTypeCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("titleLabel")]
        public Label TitleLabel { get; private set; }

        [JsonProperty("checkImage")]
        public Image CheckImage { get; private set; }

        [JsonProperty("disclosureImage")]
        public Image DisclosureImage { get; private set; }

        public DeliveryTypeCell()
        {
            Size = new Size { Width = DeviceInfo.ScreenWidth, Height = 60 };
            TitleLabel = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16)
            };
        }
    }

    public class DelivetyTimeCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("title")]
        public Label Title { get; private set; }

        public DelivetyTimeCell()
        {
            Size = new Size { Height = 44 };
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                TextAlignment = TextAlignment.Center,
                Font = Theme.FontsPalette.BoldOfSize(16),
            };
        }
    }

    public class DeliveryPointCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("checkImage")]
        public Image CheckImage { get; private set; }

        [JsonProperty("info")]
        public DeliveryPointInfo Info { get; private set; }

        public DeliveryPointCell()
        {
            Size = new Size { Width = DeviceInfo.ScreenWidth, Height = 200 };
            Info = null;
        }
    }

    public class DeliveryPointInfo
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

        [JsonProperty("mapButton")]
        public Button MapButton { get; private set; }

        [JsonProperty("routeButton")]
        public Button RouteButton { get; private set; }

        public DeliveryPointInfo()
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
            MapButton = (Button)Theme.ControlPalette.Button.Base.Clone();
            RouteButton = (Button)Theme.ControlPalette.Button.Base.Clone();
        }
    }

    public class Payments
    {
        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("cell")]
        public PaymentCell Cell { get; private set; }

        [JsonProperty("cancelButton")]
        public Button CancelButton { get; private set; }

        public Payments()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };
            Cell = new PaymentCell();
            CancelButton = (Button)Theme.ControlPalette.Button.Icon.Clone();
        }
    }

    public class PaymentCell : View
    {
        [JsonProperty("size")]
        public Size Size { get; private set; }

        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("checkImage")]
        public Image CheckImage { get; private set; }

        public PaymentCell()
        {
            Size = new Size { Height = 60 };
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16)
            };
        }
    }
}
