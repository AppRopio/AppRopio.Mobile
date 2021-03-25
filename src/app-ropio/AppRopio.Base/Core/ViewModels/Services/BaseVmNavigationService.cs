using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Core.ViewModels.Services {
    public class BaseVmNavigationService : BaseVmService, IBaseVmNavigationService
    {
        #region Services

        protected IViewModelLookupService LookupService { get { return Mvx.IoCProvider.Resolve<IViewModelLookupService>(); } }

        protected IMvxNavigationService MvxNavigationService => Mvx.IoCProvider.Resolve<IMvxNavigationService>();

        #endregion

        #region Private

        private Task Navigate(Type vmType, IMvxBundle bundle = null)
        {
            return bundle != null ?
                    MvxNavigationService.Navigate(vmType, bundle, bundle)
                        :
                    MvxNavigationService.Navigate(vmType);
        }

        #endregion

        #region Protected

        protected async Task NavigateTo<TViewModel>(IMvxBundle bundle = null)
            where TViewModel : class, IMvxViewModel
        {
            if (LookupService.IsRegistered<TViewModel>())
                try
                {
                    await Navigate(LookupService.Resolve<TViewModel>(), bundle);
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error(ex.BuildAllMessagesAndStackTrace());
                }
        }

        protected async Task NavigateTo(Type vmType, IMvxBundle bundle = null)
        {
            if (LookupService.IsRegistered(vmType))
                try
                {
                    await Navigate(LookupService.Resolve(vmType), bundle);
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error(ex.BuildAllMessagesAndStackTrace());
                }
        }

        protected async Task NavigateTo(string vmType, IMvxBundle bundle = null)
        {
            if (LookupService.IsRegistered(vmType))
                try
                {
                    await Navigate(LookupService.Resolve(vmType), bundle);
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error(ex.BuildAllMessagesAndStackTrace());
                }
        }

        #endregion

        #region Public

        public async void NavigateTo(string deeplink)
        {
            if (!deeplink.IsNullOrEmtpy())
            {
                deeplink.ParseDeeplink(out string urlScheme, out List<string> routing, out Dictionary<string, string> urlParameters);

                if (LookupService.IsRegisteredDeeplink(urlScheme))
                {
                    try
                    {
                        await MvxNavigationService.Navigate(LookupService.ResolveDeeplink(urlScheme), (IMvxBundle)new MvxBundle(urlParameters), (IMvxBundle)new MvxBundle(urlParameters));
                    }
                    catch (Exception ex)
                    {
                        Mvx.IoCProvider.Resolve<IMvxLog>().Error(ex.BuildAllMessagesAndStackTrace());
                    } 
                }
            }
        }

        #endregion
    }
}
