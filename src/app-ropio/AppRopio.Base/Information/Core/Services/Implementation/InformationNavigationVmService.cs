using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Services.Launcher;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Information.Core.ViewModels.InformationTextContent;
using AppRopio.Base.Information.Core.ViewModels.InformationWebContent;
using MvvmCross;

namespace AppRopio.Base.Information.Core.Services.Implementation
{
    public class InformationNavigationVmService : BaseVmNavigationService, IInformationNavigationVmService
    {
		public void NavigateToTextContent(BaseTextContentBundle bundle)
		{
			NavigateTo<IInformationTextContentViewModel>(bundle);
		}

		public void NavigateToWebContent(BaseWebContentBundle bundle)
		{
			NavigateTo<IInformationWebContentViewModel>(bundle);
		}

		public void NavigateToUrl(BaseWebContentBundle bundle)
		{
            Mvx.Resolve<ILauncherService>().LaunchUri(bundle.Url);
		}
    }
}