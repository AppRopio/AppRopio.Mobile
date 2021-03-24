using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.Services.Permissions;
using AppRopio.Base.Core.Services.UserDialogs;
using Foundation;
using MvvmCross;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using UIKit;

namespace AppRopio.Base.iOS.Services.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        protected IUserDialogs UserDialogs => Mvx.Resolve<IUserDialogs>();

        #region IPermissionsService implementation

        public async Task<bool> CheckPermission(Permission permission, bool goToSettingsAlert = false, string goToSettingsMessage = null)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status == PermissionStatus.Denied)
                {
                    if (goToSettingsAlert)
                    {
                        var result = await UserDialogs.Confirm(goToSettingsMessage ?? Mvx.Resolve<ILocalizationService>().GetLocalizableString("Base", "Permissions_Request"), Mvx.Resolve<ILocalizationService>().GetLocalizableString("Base", "Permissions_Go"));
                        if (result)
                        {
                            Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() =>
                            {
                                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                            });
                        }
                    }

                    return false;
                }

                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                    if (results.ContainsKey(permission))
                        status = results[permission];
                }

                return (status == PermissionStatus.Granted);
            }
            catch (Exception ex)
            {
                MvxTrace.TaggedTrace(MvxTraceLevel.Error, nameof(PermissionsService), () => ex.BuildAllMessagesAndStackTrace());
                return false;
            }
        }

        #endregion
    }
}
