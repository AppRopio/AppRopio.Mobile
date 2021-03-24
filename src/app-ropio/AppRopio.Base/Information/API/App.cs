using AppRopio.Base.API;
using AppRopio.Base.Information.API.Services;
using AppRopio.Base.Information.API.Services.Fakes;
using AppRopio.Base.Information.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.Base.Information.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
            if (ApiSettings.DebugServiceEnabled)
    			Mvx.RegisterType<IInformationService>(() => new FakeInformationService());
            else
                Mvx.RegisterType<IInformationService>(() => new InformationService());
		}
	}
}