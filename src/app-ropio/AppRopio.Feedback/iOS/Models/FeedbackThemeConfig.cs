using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.Feedback.iOS.Models
{
    public class FeedbackThemeConfig
    {
		[JsonProperty("myReviewCell")]
		public MyReviewCell MyReviewCell { get; set; }

        [JsonProperty("reviewCell")]
        public ReviewCell ReviewCell { get; set; }

		[JsonProperty("reviewButton")]
		public Button ReviewButton { get; set; }

		[JsonProperty("reviewDetails")]
        public ReviewDetails ReviewDetails { get; set; }

		[JsonProperty("reviewPost")]
        public ReviewPost ReviewPost { get; set; }

        public FeedbackThemeConfig()
        {
            MyReviewCell = new MyReviewCell();

            ReviewCell = new ReviewCell();

            ReviewButton = (Button)Theme.ControlPalette.Button.Base.Clone();

            ReviewDetails = new ReviewDetails();

            ReviewPost = new ReviewPost();
        }
    }

	public class MyReviewCell
	{
		[JsonProperty("size")]
		public Size Size { get; set; }

		[JsonProperty("title")]
		public Label Title { get; set; }

		[JsonProperty("text")]
		public Label Text { get; set; }

		[JsonProperty("date")]
		public Label Date { get; set; }

		[JsonProperty("badge")]
		public Label Badge { get; set; }

		[JsonProperty("productImage")]
		public Image ProductImage { get; private set; }

		[JsonProperty("separatorColor")]
        public Color SeparatorColor { get; private set; }

		public MyReviewCell()
		{
			Size = new Size() { Height = 114 };

            Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};

			Date = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
				Font = Theme.FontsPalette.BoldOfSize(14)
			};

			Text = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.RegularOfSize(14)
			};

			Badge = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.BoldOfSize(14),
				Background = (Color)Theme.ColorPalette.Accent.Clone(),
                Layer = new Layer()
                {
                    CornerRadius = 8
                }
			};

			ProductImage = new Image();

            SeparatorColor = (Color)Theme.ColorPalette.Separator.Clone();
		}
	}

    public class ReviewCell
    {
        [JsonProperty("size")]
        public Size Size { get; set; }

        [JsonProperty("text")]
        public Label Text { get; set; }

        [JsonProperty("author")]
        public Label Author { get; set; }

		[JsonProperty("date")]
		public Label Date { get; set; }

        [JsonProperty("badge")]
        public Label Badge { get; set; }

        public ReviewCell()
        {
            Size = new Size() { Height = 134 };

            Author = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};

			Date = new Label
			{
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14)
			};

		    Text = new Label
            {
                TextColor = (Color) Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(13)
            };

			Badge = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14),
                Background = (Color)Theme.ColorPalette.Accent.Clone(),
				Layer = new Layer()
				{
					CornerRadius = 8
				}
			};
        }
    }

    public class ReviewDetails
    {
		[JsonProperty("productImage")]
		public Image ProductImage { get; private set; }

		[JsonProperty("date")]
		public Label Date { get; set; }

		[JsonProperty("productTitle")]
		public Label ProductTitle { get; set; }

		[JsonProperty("text")]
		public Label Text { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

		[JsonProperty("editButton")]
        public Button EditButton { get; set; }

		[JsonProperty("deleteButton")]
		public Button DeleteButton { get; set; }

        public ReviewDetails()
        {
            ProductImage = new Image();

			Date = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
				Font = Theme.FontsPalette.BoldOfSize(14)
			};

			Text = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.RegularOfSize(14)
			};

            ProductTitle = new Label
            {
                TextColor = (Color) Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
            };

            Score = new Score();
            EditButton = (Button)Theme.ControlPalette.Button.Base.Clone();

            DeleteButton = (Button)Theme.ControlPalette.Button.Base.Clone();
        }
    }

    public class Score
    {
        [JsonProperty("backgroundColor")]
        public Color BackgroundColor { get; set; }

        [JsonProperty("foregroundColor")]
        public Color ForegroundColor { get; set; }

        [JsonProperty("label")]
		public Label Label { get; set; }

        public Score()
        {
            BackgroundColor = (Color)Theme.ColorPalette.DisabledControl.Clone();
            ForegroundColor = (Color)Theme.ColorPalette.Accent.Clone();

            Label = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(14),
                TextAlignment = TextAlignment.Center
			};
        }
    }

    public class ReviewPost
    {
		[JsonProperty("title")]
		public Label Title { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

		[JsonProperty("scoreValue")]
		public Label ScoreValue { get; set; }

		[JsonProperty("text")]
        public TextView Text { get; set; }

		[JsonProperty("postButton")]
		public Button PostButton { get; set; }

		[JsonProperty("accessoryNextButton")]
		public Button AccessoryNextButton { get; private set; }

        public ReviewPost()
        {
            Score = new Score();

            Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14)
			};

            ScoreValue = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.BoldOfSize(15),
                TextAlignment = TextAlignment.Center
			};

            Text = new TextView
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.RegularOfSize(13)
			};

            PostButton = (Button)Theme.ControlPalette.Button.Base.Clone();

            AccessoryNextButton = (Button)Theme.ControlPalette.Button.Accessory.Clone();
        }
    }
}