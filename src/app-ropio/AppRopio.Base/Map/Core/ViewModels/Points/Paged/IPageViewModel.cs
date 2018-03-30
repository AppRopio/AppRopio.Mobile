using AppRopio.Base.Core.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points.Paged
{
    public interface IPageViewModel : IMvxPageViewModel
    {
        IPageTitleViewModel TitleViewModel { get; }
    }
}
