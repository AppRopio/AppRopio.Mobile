using System;
using System.Windows.Input;

namespace AppRopio.ECommerce.Products.Core.ViewModels
{
    public interface IMarkedItemVM
    {
        bool Marked { get; set; }

        bool MarkEnabled { get; }

        ICommand MarkCommand { get; }
    }
}
