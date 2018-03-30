using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using AppRopio.Base.Core.Services.Permissions;
using AppRopio.Base.Core.Services.UserDialogs;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;
using Plugin.CurrentActivity;
using Plugin.Permissions.Abstractions;

namespace AppRopio.Base.Droid.Services.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        protected IUserDialogs UserDialogs => Mvx.Resolve<IUserDialogs>();

        #region IPermissionsService implementation

        public async Task<bool> CheckPermission(Permission permission, bool goToSettingsAlert = false, string goToSettingsMessage = null)
        {
            try
            {
                var status = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status == PermissionStatus.Denied)
                {
                    if (goToSettingsAlert)
                    {
                        var result = await UserDialogs.Confirm(goToSettingsMessage ?? "Для доступа к запрашиваемым разрешениям, необходимо изменить настройки приложения", "Перейти");
                        if (result)
                        {
                            Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() =>
                            {
                                var intent = new Intent();
                                intent.SetAction(Android.Provider.Settings.ActionApplicationDetailsSettings);
                                intent.SetData(Android.Net.Uri.FromParts("package", Application.Context.PackageName, null));

                                CrossCurrentActivity.Current.Activity.StartActivity(intent);
                            });
                        }
                    }

                    return false;
                }

                if (status != PermissionStatus.Granted)
                {
                    var results = await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(permission);
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
