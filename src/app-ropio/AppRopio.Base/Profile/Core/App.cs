using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Profile.Core.Services;
using AppRopio.Base.Profile.Core.Services.Implementation;
using MvvmCross;

namespace AppRopio.Base.Profile.Core
{
    public class App: MvvmCross.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
			new API.App().Initialize();

			Mvx.IoCProvider.RegisterType<IProfileVmNavigationService>(() => new ProfileVmNavigationService());
            
			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

			#endregion

			#region RouterSubscriber registration

			var routerService = Mvx.IoCProvider.Resolve<IRouterService>();

			#endregion
		}
	}
}
