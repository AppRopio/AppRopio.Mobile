using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Information.Responses;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Information.Core.ViewModels.Information
{
    public interface IInformationViewModel : IBaseViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        MvxObservableCollection<Article> Articles { get; }
    }
}