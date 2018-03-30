using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Models
{
	public class HistoryOrdersThemeConfig
	{
		public HistoryOrderCell HistoryOrderCell { get; private set; }

        public HistoryOrderStatusCell HistoryOrderStatusCell { get; private set; }

        public HistoryOrderItemCell HistoryOrderItemCell { get; private set; }

        public Label ItemsLabel { get; private set; }

        public Label InfoLabel { get; private set; }

        public Label DeliveryPointLabel { get; private set; }

        public Label DeliveryAddressLabel { get; private set; }

        public Button RepeatButton { get; private set; }

		public HistoryOrdersThemeConfig()
		{
			HistoryOrderCell = new HistoryOrderCell();
            HistoryOrderStatusCell = new HistoryOrderStatusCell();
            HistoryOrderItemCell = new HistoryOrderItemCell();

            ItemsLabel = new Label
            {
                Font = Theme.FontsPalette.SemiboldOfSize(14),
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone()
            };
            InfoLabel = new Label
            {
                Font = Theme.FontsPalette.BoldOfSize(14),
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone()
            };
            DeliveryPointLabel = new Label
            {
                Font = Theme.FontsPalette.BoldOfSize(14),
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone()
            };
            DeliveryAddressLabel = new Label
            {
                Font = Theme.FontsPalette.RegularOfSize(13),
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone()
            };

            RepeatButton = (Button)Theme.ControlPalette.Button.Base.Clone();
		}
	}

    public class HistoryOrderStatusCell : View
    {
        public Size Size { get; set; }

        public Label Title { get; set; }

        public HistoryOrderStatusCell()
        {
            Size = new Size() { Height = 21 };
			Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
			};
        }
	}

    public class HistoryOrderCell : View
    {
		public Size Size { get; private set; }

        public Image Image { get; private set; }

        public Label OrderNumber { get; private set; }

		public Label OrderStatus { get; private set; }

        public Label Price { get; private set; }

        public Label ItemsCount { get; private set; }

        public Label PaymentStatus { get; private set; }

		public HistoryOrderCell()
		{
			Size = new Size() { Height = 156 };

            Image = new Image();

			OrderNumber = new Label
			{
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
			};

            OrderStatus = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};

            Price = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
			};

            ItemsCount = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
			};

            PaymentStatus = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(12)
			};
		}
    }

    public class HistoryOrderItemCell : View
	{
        public Image Image { get; private set; }

		public Size Size { get; private set; }

		public Label Title { get; private set; }

		public Label TotalPrice { get; private set; }

		public Label Amount { get; private set; }

        public Label Badge { get; private set; }

		public HistoryOrderItemCell()
		{
			Size = new Size() { Height = 120 };

            Image = new Image();

			Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};

            TotalPrice = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
			};

            Amount = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};

            Badge = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(12),
                Background = new Color() { Hex = "F3F4F9" }
			};
		}
	}
}