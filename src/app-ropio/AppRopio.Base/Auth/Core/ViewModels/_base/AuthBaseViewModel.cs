using AppRopio.Base.Auth.Core.Models.Config;
using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.Auth.Core.ViewModels._base
{
	public abstract class AuthBaseViewModel : BaseViewModel, IAuthBaseViewModel
	{
		#region Fields

		protected AuthConfig Config { get { return Mvx.IoCProvider.Resolve<IAuthConfigService>().Config; } }

		#endregion

		#region Properties

		private bool _propertiesValid;
		public bool PropertiesValid
		{
			get
			{
				return _propertiesValid;
			}
			set
			{
				if (value != _propertiesValid)
				{
					_propertiesValid = value;
					RaisePropertyChanged(() => PropertiesValid);
				}
			}
		}

		#endregion

		#region Services

		protected new IAuthNavigationVmService NavigationVmService { get { return Mvx.IoCProvider.Resolve<IAuthNavigationVmService>(); } }

		#endregion

		#region Constructor

		public AuthBaseViewModel()
		{

		}

		#endregion

		#region Protected

		/// <summary>
		/// Проверить корректность данных в св-вах ViewModel
		/// Вызывать при изменении любого поля в VM, 
		/// </summary>
		protected void CheckPropertiesData()
		{
			PropertiesValid = IsViewModelPropertiesValid();
		}

		/// <summary>
		/// Проверять свойства, содержащие данные, полученные от пользователя
		/// </summary>
		protected abstract bool IsViewModelPropertiesValid();

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


		#endregion

	}
}
