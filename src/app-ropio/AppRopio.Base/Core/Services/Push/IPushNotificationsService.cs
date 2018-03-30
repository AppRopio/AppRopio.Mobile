using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.Core.ViewModels.Services;

namespace AppRopio.Base.Core.Services.Push
{
    public interface IPushNotificationsService : IBaseVmNavigationService
    {
        Task RegisterDeviceForPushNotificatons(string pushToken);
    }
}
