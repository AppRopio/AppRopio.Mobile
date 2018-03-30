using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public interface IDeliveryOnPointVM : IBaseDeliveryVM, ISearchViewModel
    {
        /// <summary>
        /// Точки доставки для самовывоза (магазины, постаматы и пр.)
        /// </summary>
        ObservableCollection<IDeliveryPointItemVM> DeliveryPointsItems { get; }

        /// <summary>
        /// Выбранный пункт самовывоза
        /// </summary>
        IDeliveryPointItemVM SelectedDeliveryPoint { get; }

        /// <summary>
        /// Выбор точки доставки
        /// </summary>
        ICommand DeliveryPointChangedCommand { get; }
    }
}
