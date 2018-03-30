using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using AppRopio.Base.Core.ViewModels.Services;

namespace AppRopio.Base.Core.Services.Push
{
    public class PushNotificationsService : BaseVmNavigationService, IPushNotificationsService
    {
        #region Services

        protected IPushService PushService { get { return Mvx.Resolve<IPushService>(); } }

        #endregion

        #region IPushNotificationsService implementation

        public async Task RegisterDeviceForPushNotificatons(string pushToken)
        {
            await PushService.RegisterDevice(pushToken, new System.Threading.CancellationToken());
        }

        #endregion
    }
}
