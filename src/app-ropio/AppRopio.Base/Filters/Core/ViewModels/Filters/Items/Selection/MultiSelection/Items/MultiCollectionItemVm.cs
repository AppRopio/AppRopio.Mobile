using System;
using MvvmCross.ViewModels;
namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection.Items
{
    public class MultiCollectionItemVM : MvxViewModel
    {
        public string Id { get; private set; }

        public string ValueName { get; private set; }

        public MultiCollectionItemVM(string id, string valueName)
        {
            Id = id;
            ValueName = valueName;
        }
    }
}
