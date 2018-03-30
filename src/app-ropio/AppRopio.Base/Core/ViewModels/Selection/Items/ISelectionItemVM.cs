using System;
using MvvmCross.Core.ViewModels;
namespace AppRopio.Base.Core.ViewModels.Selection.Items
{
    public interface ISelectionItemVM : IMvxViewModel
    {
        string Id { get; }

        bool Selected { get; set; }

        string ValueName { get; }
    }
}
