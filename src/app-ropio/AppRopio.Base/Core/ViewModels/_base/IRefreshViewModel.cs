using System;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Core.ViewModels
{
    public interface IRefreshViewModel : ILoadMoreViewModel
    {
        IMvxCommand ReloadCommand { get; }

        bool Reloading { get; }
    }
}
