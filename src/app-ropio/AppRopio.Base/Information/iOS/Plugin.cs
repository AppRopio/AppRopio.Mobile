using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Information.Core;
using AppRopio.Base.Information.Core.ViewModels.Information;
using AppRopio.Base.Information.Core.ViewModels.InformationTextContent;
using AppRopio.Base.Information.Core.ViewModels.InformationWebContent;
using AppRopio.Base.Information.iOS.Services;
using AppRopio.Base.Information.iOS.Services.Implementation;
using AppRopio.Base.Information.iOS.Views;
using AppRopio.Base.Information.iOS.Views.InformationTextContent;
using AppRopio.Base.Information.iOS.Views.InformationWebContent;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Information.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Information";

        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IInformationThemeConfigService>(() => new InformationThemeConfigService());

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<IInformationViewModel, InformationViewController>();
            viewLookupService.Register<IInformationTextContentViewModel, InformationTextContentViewController>();
            viewLookupService.Register<IInformationWebContentViewModel, InformationWebContentViewController>();
        }
    }
}