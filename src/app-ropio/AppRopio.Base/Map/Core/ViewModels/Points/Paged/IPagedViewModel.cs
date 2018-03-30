using AppRopio.Base.Core.ViewModels;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace AppRopio.Base.Map.Core.ViewModels.Points.Paged
{
    public interface IPagedViewModel : IBaseViewModel
    {
        int CurrentPage { get; }

        ObservableCollection<IPageTitleViewModel> TitleViewModels { get; }

        ObservableCollection<IPageViewModel> Pages { get; }

        ICommand TitleSelectedCommand { get; }

        ICommand PageChangedCommand { get; }
    }
}
