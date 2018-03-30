using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Header
{
    public class CatalogSortFiltersHeaderVM : MvxViewModel
    {
        #region Commands

        private ICommand _sortCommand;
        public ICommand SortCommand
        {
            get
            {
                return _sortCommand ?? (_sortCommand = new MvxCommand(OnSortExecute));
            }
        }

        private ICommand _filtersCommand;
        public ICommand FiltersCommand
        {
            get
            {
                return _filtersCommand ?? (_filtersCommand = new MvxCommand(OnFiltersExecute));
            }
        }

        #endregion

        #region Properties

        public Action SortExecute { get; set; }

        public Action FiltersExecute { get; set; }

        #endregion

        #region Protected

        protected virtual void OnSortExecute()
        {
            SortExecute?.Invoke();
        }

        protected virtual void OnFiltersExecute()
        {
            FiltersExecute?.Invoke();
        }

        #endregion
    }
}
