using System;
using System.Text.RegularExpressions;
using AppRopio.Base.Auth.Core;
using AppRopio.Base.Auth.Core.ViewModels.SignUp;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using AppRopio.Base.Auth.iOS.Views._base;
using AppRopio.Base.Auth.iOS.Views.SignUp.Cells;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Models.Auth.Enums;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Views.SignUp
{
    public partial class SignUpViewController : AuthBaseViewController<ISignUpViewModel>
	{
		#region Properties

		protected override string AccessoryButtonTitle
		{
			get
			{
                return ViewModel.SignUpText;
			}
		}

		#endregion

		#region Constructor

		public SignUpViewController() 
            : base("SignUpViewController", null)
		{
			RegisterKeyboardActions = true;
		}

		#endregion

		#region Protected

		protected override void OnNextButtonClick(object sender, EventArgs e)
		{
			View?.EndEditing(true);
		}

		protected virtual UITableViewCell CellFabric(UITableView tableView, Foundation.NSIndexPath indexPath, ISignUpItemBaseViewModel model)
		{
			UITableViewCell cell = null;

			switch (model.Type)
			{
				case RegistrationFieldType.Picker:
					var pickerCell = tableView.DequeueReusableCell(SignUpItemPickerViewCell.Key, indexPath) as SignUpItemPickerViewCell;

					pickerCell.TextField.Placeholder = model.Placeholder;
					cell = pickerCell;
					break;

				case RegistrationFieldType.Date:
					var dateCell = tableView.DequeueReusableCell(SignUpItemDatePickerViewCell.Key, indexPath) as SignUpItemDatePickerViewCell;

					dateCell.TextField.Placeholder = model.Placeholder;
					cell = dateCell;
					break;

				default:
					var baseCell = tableView.DequeueReusableCell(SignUpItemBaseViewCell.Key, indexPath) as SignUpItemBaseViewCell;
					baseCell.TextField.Placeholder = model.Placeholder;

					baseCell.TextField.InputAccessoryView = _accessoryButton;

					baseCell.TextField.ShouldReturn += (textField) =>
					{
						baseCell.TextField?.ResignFirstResponder();
						return true;
					};

					switch (model.RegistrationField.Type)
					{
						case RegistrationFieldType.TextField:
							baseCell.TextField.KeyboardType = UIKeyboardType.Default;
							break;
						case RegistrationFieldType.Phone:
							baseCell.TextField.KeyboardType = UIKeyboardType.NumberPad;
							break;

						case RegistrationFieldType.Email:
							baseCell.TextField.KeyboardType = UIKeyboardType.EmailAddress;
							break;

						case RegistrationFieldType.Password:
							baseCell.TextField.KeyboardType = UIKeyboardType.Default;
							baseCell.TextField.SecureTextEntry = true;
							baseCell.TextField.SetupSecurity();
							break;
					}
					cell = baseCell;
					break;
			}

			return cell;
		}

		protected virtual void SetLegalText(UITextView textView)
		{
            var legalText = Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "TermsText");

			string pattern = @"(\[.*\])";
			Regex regex = new Regex(pattern);

			Match match = regex.Match(legalText);

			if (!Config.LegalUrl.IsNullOrEmtpy() && match.Success)
			{
				var legalUrlText = match.Groups[1].Value;

				var range = new NSRange(legalText.IndexOf(legalUrlText), legalUrlText.Length - 2);

				var paragraph = new NSMutableParagraphStyle();
				paragraph.Alignment = UITextAlignment.Center;

				var attributedText = new NSMutableAttributedString(legalText.Replace("[", "").Replace("]", ""), (UIFont)ThemeConfig.LegalText.Font, ThemeConfig.LegalText.TextColor.ToUIColor(), paragraphStyle: paragraph);
				attributedText.AddAttributes(new UIStringAttributes
				{
					ForegroundColor = ThemeConfig.LegalText.TextColor.ToUIColor(),
					UnderlineStyle = NSUnderlineStyle.Single,
					UnderlineColor = ThemeConfig.LegalText.TextColor.ToUIColor(),
					Link = NSUrl.FromString(@""),
					StrokeColor = ThemeConfig.LegalText.TextColor.ToUIColor(),
				}, range);

				textView.Font = (UIFont)ThemeConfig.LegalText.Font;
				textView.TextColor = ThemeConfig.LegalText.TextColor.ToUIColor();
				textView.TintColor = ThemeConfig.LegalText.TextColor.ToUIColor();
				textView.AttributedText = attributedText;

				textView.AllowUrlInteraction = (tView, url, characterRange, interaction) =>
				{
					OpenBrowser(Config.LegalUrl);
					return false;
				};
				textView.SetContentOffset(new CoreGraphics.CGPoint(), false);
				textView.TextContainerInset = new UIEdgeInsets();
				textView.ContentInset = new UIEdgeInsets();
			}
			else
			{
				if (legalText.IsNullOrEmtpy())
					textView.Hidden = true;
				else
				{
					textView.Font = (UIFont)ThemeConfig.LegalText.Font;
					textView.TextColor = ThemeConfig.LegalText.TextColor.ToUIColor();
					textView.TintColor = ThemeConfig.LegalText.TextColor.ToUIColor();
					textView.Text = legalText.Replace("[", "").Replace("]", "");
				}
			}
		}

		#region InitializationControls

		protected virtual void SetupImage(UIImageView imageView)
		{
			imageView.SetupStyle(ThemeConfig.SignUpImage);
			_imageWidth.Constant = imageView.Image.Size.Width;
			_imageHeight.Constant = imageView.Image.Size.Height;
		}

		protected virtual void SetupTitleLabel(UILabel label)
		{
			label.SetupStyle(ThemeConfig.Title);
            label.Text = Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "SignUp_Join");
		}

		protected virtual void RegisterTableCells(UITableView tableView)
		{
			tableView.RegisterNibForCellReuse(SignUpItemBaseViewCell.Nib, SignUpItemBaseViewCell.Key);
			tableView.RegisterNibForCellReuse(SignUpItemPickerViewCell.Nib, SignUpItemPickerViewCell.Key);
			tableView.RegisterNibForCellReuse(SignUpItemDatePickerViewCell.Nib, SignUpItemDatePickerViewCell.Key);
		}

		protected virtual void SetupTableView(UITableView tableView)
		{
			tableView.RowHeight = UITableView.AutomaticDimension;
			tableView.EstimatedRowHeight = 82;
		}

		protected virtual void SetupNextBtn(UIButton button)
		{
			button.TouchUpInside += OnNextButtonClick;
			button.SetupStyle(ThemeConfig.Button);
		}

		#endregion

		#region BindingControls

		protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<SignUpViewController, ISignUpViewModel> set)
		{
			var dataSource = SetupCollectionDataSource(tableView);

			set.Bind(dataSource).To(vm => vm.Items);

			tableView.Source = dataSource;

			tableView.ReloadData();
		}

		protected virtual MvxTableViewSource SetupCollectionDataSource(UITableView tableView)
		{
			var dataSource = new SignUpTableViewSource(tableView, CellFabric);

			return dataSource;
		}

		protected virtual void BindAccessoryBtn(UIButton button, MvxFluentBindingDescriptionSet<SignUpViewController, ISignUpViewModel> set)
		{
            set.Bind(button).To(vm => vm.SignUpCommand);
            set.Bind(button).For("Title").To(vm => vm.SignUpText);
			set.Bind(button).For(p => p.Enabled).To(vm => vm.PropertiesValid);
		}

		protected virtual void BindNextBtn(UIButton button, MvxFluentBindingDescriptionSet<SignUpViewController, ISignUpViewModel> set)
		{
			set.Bind(button).To(vm => vm.SignUpCommand);
            set.Bind(button).For("Title").To(vm => vm.SignUpText);
			set.Bind(button).For(p => p.Enabled).To(vm => vm.PropertiesValid);
		}

		#endregion

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
			base.InitializeControls();

            Title = Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "SignUp_Title");

			SetupImage(_iconImage);

			SetupTitleLabel(_titleLabel);

			SetLegalText(_legalTextView);

			SetupNextBtn(_nextButton);

			SetupTableView(_tableView);

			RegisterTableCells(_tableView);
		}

		protected override void BindControls()
		{
			var set = this.CreateBindingSet<SignUpViewController, ISignUpViewModel>();

			BindAccessoryBtn(_accessoryButton, set);
			BindNextBtn(_nextButton, set);
			BindTableView(_tableView, set);

			set.Apply();
		}

		protected override void CleanUp()
		{
			base.CleanUp();
			ReleaseDesignerOutlets();
		}

		protected override void KeyboardWillShowNotification(NSNotification notification)
		{
			_nextButton.Hidden = true;
			base.KeyboardWillShowNotification(notification);
		}

		protected override void KeyboardWillHideNotification(NSNotification notification)
		{
			_nextButton.Hidden = false;
			base.KeyboardWillHideNotification(notification);
		}

		#endregion

		#endregion

		#region Public

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController?.SetNavigationBarHidden(false, true);
			_tableView?.UpdateHeaderHeight();
		}

		#endregion

		#region OpenBrowser

		private async void OpenBrowser(string url)
		{
			try
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
				{
					var sfViewController = new SafariServices.SFSafariViewController(new NSUrl(url), true);
					var vc = GetVisibleViewController();

					if (sfViewController.PopoverPresentationController != null)
						sfViewController.PopoverPresentationController.SourceView = vc.View;

					await vc.PresentViewControllerAsync(sfViewController, true);
				}
				else
				{
                    if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl(url)))
    					UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
				}
			}
			catch (Exception ex)
			{
				Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"{GetType().FullName}:\n{ex.StackTrace}");
			}

		}

		private UIViewController GetVisibleViewController()
		{
			var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

			if (rootController.PresentedViewController == null)
				return rootController;

			if (rootController.PresentedViewController is UINavigationController)
				return ((UINavigationController)rootController.PresentedViewController).TopViewController;

			if (rootController.PresentedViewController is UITabBarController)
				return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;

			return rootController.PresentedViewController.PresentedViewController ?? rootController.PresentedViewController;
		}

		#endregion
	}
}

