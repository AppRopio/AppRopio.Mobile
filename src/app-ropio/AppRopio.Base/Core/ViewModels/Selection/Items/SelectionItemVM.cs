using System;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Core.ViewModels.Selection.Items
{
    public class SelectionItemVM : MvxViewModel, ISelectionItemVM
    {
        public string Id { get; private set; }

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                RaisePropertyChanged(() => Selected);
            }
        }

        public string ValueName { get; private set; }

        public SelectionItemVM(string id, string valueName, bool selected)
        {
            Id = id;
            ValueName = valueName;
            Selected = selected;
        }
    }
}
