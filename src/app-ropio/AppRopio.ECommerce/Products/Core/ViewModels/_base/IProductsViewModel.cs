using System;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels
{
    public interface IProductsViewModel : IBaseViewModel
    {
        ICommand SelectionChangedCommand { get; }

        bool SearchEnabled { get; }

        bool SearchBar { get; }

        ICommand ShowSearchCommand { get; }

        IMvxViewModel CartIndicatorVM { get; }
    }
}

