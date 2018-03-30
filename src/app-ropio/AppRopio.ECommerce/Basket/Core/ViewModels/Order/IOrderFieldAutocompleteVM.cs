using System;
using System.Collections.ObjectModel;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using MvvmCross.Core.ViewModels;
namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order
{
    public interface IOrderFieldAutocompleteVM : IBaseViewModel
    {
        /// <summary>
        /// Поле, для которого используется автокомплит
        /// </summary>
        IOrderFieldItemVM OrderFieldItem { get; }

        /// <summary>
        /// Изменение значения для обновления списка автокомплита
        /// </summary>
        IMvxCommand ValueChangedCommand { get; }

        /// <summary>
        /// Список вариантов автокомплита
        /// </summary>
        ObservableCollection<IOrderFieldAutocompleteItemVM> Items { get; }

        /// <summary>
        /// Выбор значения автокомплита
        /// </summary>
        IMvxCommand SelectionChangedCommand { get; }

        /// <summary>
        /// Применение автокомплита
        /// </summary>
        IMvxCommand ApplyCommand { get; }
    }
}
