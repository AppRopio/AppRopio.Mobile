using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Widget;
using Android.Views;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Navigation.Menu.Core.Models;
using AppRopio.Navigation.Menu.Core.Services;
using AppRopio.Navigation.Menu.Core.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace AppRopio.Navigation.Menu.Droid.Views
{
    [Activity(
        LaunchMode = LaunchMode.SingleTask,
        AlwaysRetainTaskState = true,
        WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden | SoftInput.StateAlwaysHidden/*,
        ScreenOrientation = ScreenOrientation.Portrait*/)]
    public class MenuActivity : BaseDrawerActivity<IMenuViewModel>
    {
        #region Properties

        protected MenuConfig Config => Mvx.IoCProvider.Resolve<IMenuConfigService>().Config;

        #endregion

        #region Constructor

        public MenuActivity()
            : base(Resource.Layout.app_activity_drawer, Resource.Id.app_drawer_layout)
        {
            
        }

        #endregion

        #region Protected

        protected override void InitStartPage()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxNavigationService>(async () => 
            {
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate(ViewModel.DefaultViewModel, ((IMvxBundle)new BaseBundle(Base.Core.Models.Navigation.NavigationType.ClearAndPush)), null);
            });
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //bool vkResult;
            //var task = VKSdk.OnActivityResultAsync(requestCode, resultCode, data, out vkResult);
            //var oauthService = Mvx.IoCProvider.Resolve<IOAuthService>() as OAuthService;
            //if (!vkResult)
            //{
            //	base.OnActivityResult(requestCode, resultCode, data);
            //	FacebookService.Instance.OnActivityResult(requestCode, (int)resultCode, data);
            //	return;
            //}
            //try
            //{
            //	var token = await task;
            //	oauthService.SetVkToken(token);
            //}
            //catch (Exception e)
            //{
            //	var vkException = e as VKException;
            //	if (vkException == null || vkException.Error.ErrorCode != VKontakte.API.VKError.VkCanceled)
            //		oauthService.SetException(e);
            //	else
            //		oauthService.SetCanceled();
            //}
        }

        #endregion

        public void LockDrawer()
        {
            Drawer.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
        }

        public void UnlockDrawer()
        {
            Drawer.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
        }
    }
}
