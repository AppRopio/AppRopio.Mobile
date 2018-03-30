using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.Base.Core.ViewModels.Selection.Items;

namespace AppRopio.Base.Core.ViewModels.Selection.Services
{
    public interface IBaseSelectionVmService<TValue, TSelectedValue>
        where TValue : class
        where TSelectedValue : class
    {
        Task<ObservableCollection<ISelectionItemVM>> LoadItemsFor(List<TValue> allValues, List<TSelectedValue> selectedValues, string searchText);
    }
}
