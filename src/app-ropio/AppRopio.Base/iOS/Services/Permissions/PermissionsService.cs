using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.Services.Permissions;
using AppRopio.Base.Core.Services.UserDialogs;
using Foundation;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Logging;
using UIKit;
using Xamarin.Essentials;

namespace AppRopio.Base.iOS.Services.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        protected IUserDialogs UserDialogs => Mvx.IoCProvider.Resolve<IUserDialogs>();

        #region IPermissionsService implementation

        public async Task<bool> CheckPermission(Xamarin.Essentials.Permissions.BasePermission permission, bool goToSettingsAlert = false, string goToSettingsMessage = null)
        {
            try
            {
                var status = await permission.CheckStatusAsync();
                if (status == PermissionStatus.Denied)
                {
                    if (goToSettingsAlert)
                    {
                        var result = await UserDialogs.Confirm(goToSettingsMessage ?? Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString("Base", "Permissions_Request"), Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString("Base", "Permissions_Go"));
                        if (result)
                        {
                            Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
                            {
                                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                            });
                        }
                    }

                    return false;
                }

                if (status != PermissionStatus.Granted)
                {
                    status = await permission.RequestAsync();
                }

                return (status == PermissionStatus.Granted);
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{nameof(PermissionsService)}: {ex.BuildAllMessagesAndStackTrace()}");
                return false;
            }
        }

        #endregion
    }
}
