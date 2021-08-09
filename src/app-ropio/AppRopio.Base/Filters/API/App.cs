using AppRopio.Base.API;
using AppRopio.Base.Filters.API.Services.Fakes;
using AppRopio.Base.Filters.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.Base.Filters.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterType<IFiltersService>(() => new FiltersFakeService());
            else
                Mvx.IoCProvider.RegisterType<IFiltersService>(() => new FiltersService());
        }
    }
}

