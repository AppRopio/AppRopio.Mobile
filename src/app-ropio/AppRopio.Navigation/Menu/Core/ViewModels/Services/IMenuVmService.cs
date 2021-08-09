using System.Collections.Generic;
using AppRopio.Navigation.Menu.Core.ViewModels.Items;
using MvvmCross.ViewModels;
using System;

namespace AppRopio.Navigation.Menu.Core.ViewModels.Services
{
    public interface IMenuVmService
    {
        Type DefaultViewModelType();

        /// <summary>
        /// Loads the header view model if exist.
        /// </summary>
        /// <returns>The header view model if exist, overwise null</returns>
        IMvxViewModel LoadHeaderVmIfExist();
        
        ICollection<IMenuItemVM> LoadItemsFromConfigJSON();

        /// <summary>
        /// Loads the footer view model if exist.
        /// </summary>
        /// <returns>The footer view model if exist, overwise null</returns>
        IMvxViewModel LoadFooterVmIfExist();
    }
}

