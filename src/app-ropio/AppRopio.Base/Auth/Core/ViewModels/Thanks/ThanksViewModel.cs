using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.PresentationHints;
using AppRopio.Base.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Auth.Core.ViewModels.Thanks
{
	public class ThanksViewModel : BaseViewModel, IThanksViewModel
	{
		public ICommand StartCommand
		{
			get
			{
				return new MvxCommand(OnStartCommandExecute);
			}
		}

		public ThanksViewModel()
		{
			VmNavigationType = Base.Core.Models.Navigation.NavigationType.PresentModal;
		}

		#region protected

		public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

			var navigationBundle = parameters.ReadAs<BaseBundle>();
			this.InitFromBundle(navigationBundle);
		}

		protected virtual void InitFromBundle(BaseBundle parameters)
		{
			VmNavigationType = parameters.NavigationType;
		}

		protected virtual void OnStartCommandExecute()
		{
			ChangePresentation(new NavigateToDefaultViewModelHint());
		}

		#endregion
	}
}
