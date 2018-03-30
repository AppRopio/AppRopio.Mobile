using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Horizontal
{
    public class HorizontalCollectionFiVm : BaseCollectionFiVm, IHorizontalCollectionFiVm
    {
        public HorizontalCollectionFiVm(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
            : base(filter, applyedFilterValues)
        {

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Items = new ObservableCollection<CollectionItemVM>(Items.OrderByDescending(x => x.Selected));
        }

        protected override void OnItemSelected(CollectionItemVM item)
        {
            var oldIndex = Items.IndexOf(item);

            var dataSource = new ObservableCollection<CollectionItemVM>(Items);

            var lastSelectedItem = Items.LastOrDefault(x => x.Selected);
            var newIndex = lastSelectedItem == null ? 0 : Items.IndexOf(lastSelectedItem) + 1;

            item.Selected = !item.Selected;

            Items.Move(oldIndex, newIndex);

            Task.Run(BuildSelectedValue);
        }
    }
}
