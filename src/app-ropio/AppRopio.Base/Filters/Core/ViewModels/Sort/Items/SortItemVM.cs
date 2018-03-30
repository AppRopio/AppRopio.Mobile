using System;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Filters.Core.ViewModels.Sort.Items
{
    public class SortItemVM : MvxViewModel, ISortItemVM
    {
        public SortType Sort { get; private set; }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

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

        public SortItemVM(SortType model, bool selected)
        {
            Sort = model;

            Name = model.Name;

            Selected = selected;
        }
    }
}
