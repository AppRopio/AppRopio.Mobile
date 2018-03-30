using System;
using System.Windows.Input;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax
{
    public interface IBaseMinMaxFiVm : IFiltersItemVM
    {
        ICommand MinValueChangedCommand { get; }

        ICommand MaxValueChangedCommand { get; }
    }

    public interface IBaseMinMaxFiVm<T> : IBaseMinMaxFiVm
        where T : IComparable, IComparable<T>, IEquatable<T>
    {
        T AbsoluteMin { get; }

        T AbsoluteMax { get; }

        T Min { get; }

        T Max { get; }
    }
}
