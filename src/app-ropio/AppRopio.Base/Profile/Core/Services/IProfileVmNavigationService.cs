using System;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Core.Models.Bundle;
namespace AppRopio.Base.Profile.Core.Services
{
    public interface IProfileVmNavigationService : IBaseVmNavigationService
    {
        void NavigateToProfile(BaseBundle bundle);

        void NavigateToAuthorization(BaseBundle bundle);
    }
}
