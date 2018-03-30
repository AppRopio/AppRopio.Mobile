using System;
namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Switch
{
    public interface ISwitchFiVm : IFiltersItemVM, ISelectableFilterItemVM
    {
        bool Enabled { get; }
    }
}
