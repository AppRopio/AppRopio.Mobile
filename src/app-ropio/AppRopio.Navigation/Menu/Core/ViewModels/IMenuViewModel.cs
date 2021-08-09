using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Navigation.Menu.Core.ViewModels.Items;
using MvvmCross.ViewModels;

namespace AppRopio.Navigation.Menu.Core.ViewModels
{
    public interface IMenuViewModel : IBaseViewModel
    {
        /// <summary>
        /// Хедер списка
        /// </summary>
        IMvxViewModel HeaderVm { get; set; }

        ICommand SelectionChangedCommand { get; }

        /// <summary>
        /// Список ячеек в меню
        /// </summary>
        MvxObservableCollection<IMenuItemVM> Items { get; set; }

        /// <summary>
        /// Футер списка
        /// </summary>
        IMvxViewModel FooterVm { get; set; }

        /// <summary>
        /// The default view model for every app start
        /// </summary>
        Type DefaultViewModel { get; }
    }
}

