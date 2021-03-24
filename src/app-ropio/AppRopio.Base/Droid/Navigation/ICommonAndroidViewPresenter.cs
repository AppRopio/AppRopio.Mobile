using System;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Android.Views;

namespace AppRopio.Base.Droid.Navigation
{
    public interface ICommonAndroidViewPresenter : IMvxAndroidViewPresenter, IMvxMultipleViewModelCache
    {
        bool CanPop();

        void MoveBack();
    }
}
