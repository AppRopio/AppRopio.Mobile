using AppRopio.Base.API;
using AppRopio.Base.Filters.API.Services.Fakes;
using AppRopio.Base.Filters.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.Base.Filters.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterType<IFiltersService>(() => new FiltersFakeService());
            else
                Mvx.RegisterType<IFiltersService>(() => new FiltersService());
        }
    }
}

