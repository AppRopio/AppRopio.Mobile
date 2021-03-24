using System;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross;
using AppRopio.Base.Profile.Core.Services;
using AppRopio.Base.Profile.Core.Services.Implementation;

namespace AppRopio.Base.Profile.Core
{
	public class App: MvvmCross.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
            Mvx.RegisterSingleton<IProfileVmNavigationService>(new ProfileVmNavigationService());
            
			#region VMs registration

			var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

			#endregion

			#region RouterSubscriber registration

			var routerService = Mvx.Resolve<IRouterService>();

			#endregion
		}
	}
}
