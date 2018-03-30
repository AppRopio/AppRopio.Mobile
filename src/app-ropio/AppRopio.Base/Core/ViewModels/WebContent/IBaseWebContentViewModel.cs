using System;
using System.Windows.Input;
namespace AppRopio.Base.Core.ViewModels.WebContent
{
    public interface IBaseWebContentViewModel : IBaseViewModel
    {
        ICommand LoadFinishedCommand { get; }

        string Title { get; }

        string Url { get; }

        string HtmlContent { get; }
    }
}
