using System;
using System.Windows.Input;

namespace AppRopio.Base.Core.ViewModels.Search
{
    public interface ISearchViewModel : IBaseViewModel
    {
        string SearchText { get; set; }

        ICommand SearchCommand { get; }

        ICommand CancelSearchCommand { get; }
    }
}
