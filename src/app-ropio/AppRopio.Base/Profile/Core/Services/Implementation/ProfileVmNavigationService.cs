using System;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Auth.Core.Services;
using MvvmCross;
using AppRopio.Base.Profile.Core.ViewModels.Profile;
namespace AppRopio.Base.Profile.Core.Services.Implementation
{
    public class ProfileVmNavigationService : BaseVmNavigationService, IProfileVmNavigationService
    {
        #region Services

        protected IAuthNavigationVmService AuthNavigationVmService => Mvx.Resolve<IAuthNavigationVmService>();

        #endregion

        public void NavigateToAuthorization(BaseBundle bundle)
        {
            AuthNavigationVmService.NavigateToAuthorization(bundle);
        }

        public void NavigateToProfile(BaseBundle bundle)
        {
            NavigateTo<IProfileViewModel>(bundle);
        }
    }
}
