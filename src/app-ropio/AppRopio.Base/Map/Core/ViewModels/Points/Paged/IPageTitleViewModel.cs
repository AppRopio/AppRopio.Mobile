using MvvmCross.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points.Paged
{
    public interface IPageTitleViewModel : IMvxViewModel
    {
        string Title { get; }

        bool IsSelected { get; set; }
    }
}
