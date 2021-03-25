using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.Services.Permissions;
using AppRopio.Base.Core.Services.UserDialogs;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Logging;

using Xamarin.Essentials;

namespace AppRopio.Base.Droid.Services.Permissions
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
                                var intent = new Intent();
                                intent.SetAction(Android.Provider.Settings.ActionApplicationDetailsSettings);
                                intent.SetData(Android.Net.Uri.FromParts("package", Application.Context.PackageName, null));

                                Platform.CurrentActivity.StartActivity(intent);
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
