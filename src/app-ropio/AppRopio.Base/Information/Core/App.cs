using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Information.Core.Services;
using AppRopio.Base.Information.Core.Services.Implementation;
using AppRopio.Base.Information.Core.ViewModels.Information;
using AppRopio.Base.Information.Core.ViewModels.Information.Services;
using AppRopio.Base.Information.Core.ViewModels.InformationTextContent;
using AppRopio.Base.Information.Core.ViewModels.InformationWebContent;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.Information.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
            new API.App().Initialize();

			Mvx.IoCProvider.RegisterSingleton<IInformationVmService>(() => new InformationVmService());

            Mvx.IoCProvider.RegisterType<IInformationNavigationVmService>(() => new InformationNavigationVmService());

			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

			vmLookupService.Register<IInformationViewModel, InformationViewModel>();
            vmLookupService.Register<IInformationTextContentViewModel, InformationTextContentViewModel>();
            vmLookupService.Register<IInformationWebContentViewModel, InformationWebContentViewModel>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
			routerService.Register<IInformationViewModel>(new InformationRouterSubscriber());
		}
	}
}
