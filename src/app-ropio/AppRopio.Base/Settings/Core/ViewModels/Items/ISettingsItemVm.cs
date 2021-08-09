using System;
using AppRopio.Base.Settings.Core.Models;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Settings.Core.ViewModels.Items
{
    public interface ISettingsItemVm : IMvxViewModel
    {
        string Title { get; }

        SettingsElementType ElementType { get; }
    }
}