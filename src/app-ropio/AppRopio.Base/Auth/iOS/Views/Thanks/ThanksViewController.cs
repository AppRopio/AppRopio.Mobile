using AppRopio.Base.Auth.Core.ViewModels.Thanks;
using AppRopio.Base.Auth.iOS.Models;
using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Views.Thanks
{
	public partial class ThanksViewController : CommonViewController<IThanksViewModel>
	{
		protected AuthThemeConfig ThemeConfig { get { return Mvx.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

		public ThanksViewController() : base("ThanksViewController", null)
		{
		}

		#region Protected

		#region InitializationControls

		protected virtual void SetupImageView(UIImageView imageView)
		{
			imageView.SetupStyle(ThemeConfig.ThanksImage);
			_imageWidth.Constant = imageView.Image.Size.Width;
			_imageHeight.Constant = imageView.Image.Size.Height;

		}

		protected virtual void SetupTitleLabel(UILabel label)
		{
			label.SetupStyle(ThemeConfig.Title);
			label.Text = "Спасибо!";
		}

		protected virtual void SetupDesriptionLabel(UILabel label)
		{
			label.SetupStyle(ThemeConfig.Description);
			label.Text = "Мы рады, что вы с нами.\nНачинайте совершать покупки.";
		}

		protected virtual void SetupDoneButton(UIButton button)
		{
			button.SetupStyle(ThemeConfig.Button);
			button.WithTitleForAllStates("Начать".ToUpper());
		}

		#endregion

		#region BindingControls

		protected virtual void BindDoneBtn(UIButton button, MvxFluentBindingDescriptionSet<ThanksViewController, IThanksViewModel> set)
		{
			set.Bind(button).To(vm => vm.StartCommand);
		}

		#endregion

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
			SetupImageView(_iconImage);
			SetupTitleLabel(_titleLabel);
			SetupDesriptionLabel(_descriptionLabel);
			SetupDoneButton(_doneButton);
		}

		protected override void BindControls()
		{
			var set = this.CreateBindingSet<ThanksViewController, IThanksViewModel>();
			BindDoneBtn(_doneButton, set);
			set.Apply();
		}

		protected override void CleanUp()
		{
			base.CleanUp();
			ReleaseDesignerOutlets();
		}

		#endregion

		#endregion

	}
}

