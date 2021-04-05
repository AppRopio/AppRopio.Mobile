using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Core.ViewModels.Services
{
    public interface IBaseVmNavigationService
    {
        Task NavigateTo(string deeplink);

        Task ChangePresentation(MvxPresentationHint hint);

        Task Close(IMvxViewModel viewModel);
    }
}