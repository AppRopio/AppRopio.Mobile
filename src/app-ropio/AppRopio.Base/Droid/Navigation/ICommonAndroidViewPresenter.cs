using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace AppRopio.Base.Droid.Navigation
{
    public interface ICommonAndroidViewPresenter : IMvxAndroidViewPresenter, IMvxMultipleViewModelCache
    {
        bool CanPop();

        void MoveBack();
    }
}
