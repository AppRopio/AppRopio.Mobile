using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Models
{
	public class AuthThemeConfig
	{
		#region Properties

		[JsonProperty("secureTextImage")]
		public Image SecureTextImage { get; private set; }

		[JsonProperty("authLogoImage")]
		public Image AuthLogoImage { get; private set; }

		[JsonProperty("signInImage")]
		public Image SignInImage { get; private set; }

		[JsonProperty("newPasswordImage")]
		public Image NewPasswordImage { get; private set; }

		[JsonProperty("resetPasswordImage")]
		public Image ResetPasswordImage { get; private set; }

		[JsonProperty("resetPasswordSendedImage")]
		public Image ResetPasswordSendedImage { get; private set; }

		[JsonProperty("signUpImage")]
		public Image SignUpImage { get; private set; }

		[JsonProperty("thanksImage")]
		public Image ThanksImage { get; private set; }

		[JsonProperty("pickerCell")]
		public PickerCell PickerCell { get; private set; }

		[JsonProperty("legalText")]
		public Label LegalText { get; private set; }

		[JsonProperty("description")]
		public Label Description { get; private set; }

		[JsonProperty("descriptionTitle")]
		public Label Title { get; private set; }

		[JsonProperty("textField")]
		public TextField TextField { get; private set; }

		[JsonProperty("button")]
		public Button Button { get; private set; }

		[JsonProperty("accessoryButton")]
		public Button AccessoryButton { get; private set; }

		[JsonProperty("textButton")]
		public Button TextButton { get; private set; }

		[JsonProperty("closeIconButton")]
		public Button CloseIconButton { get; private set; }

		[JsonProperty("vkButton")]
		public Button VkButton { get; private set; }

		[JsonProperty("facebookButton")]
		public Button FacebookButton { get; private set; }

		#endregion

		public AuthThemeConfig()
		{
			LegalText = new Label()
			{
				TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
				Font = new Font { Name = Theme.FontsPalette.Regular, Size = 14 }
			};

			Description = new Label()
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = new Font { Name = Theme.FontsPalette.Regular, Size = 16 },
                TextAlignment = TextAlignment.Center
			};

			Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = new Font { Name = Theme.FontsPalette.Bold, Size = 16 },
                TextAlignment = TextAlignment.Center
			};

			PickerCell = new PickerCell();

			TextField = (TextField)Theme.ControlPalette.TextField.Clone();

            Button = (Button)Theme.ControlPalette.Button.Base.Clone();

            TextButton = (Button)Theme.ControlPalette.Button.Text.Clone();

            CloseIconButton = (Button)Theme.ControlPalette.Button.Text.Clone();

            AccessoryButton = (Button)Theme.ControlPalette.Button.Accessory.Clone();

			SecureTextImage = new Image();

			VkButton = new Button();

			FacebookButton = new Button();
		}
	}
}
