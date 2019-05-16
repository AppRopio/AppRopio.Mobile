using System.Windows.Input;

namespace AppRopio.Base.Core.ViewModels._base
{
    public interface IActionCommandViewModel
    {
        ICommand ActionCommand { get; }

        string ActionText { get; }
    }
}
