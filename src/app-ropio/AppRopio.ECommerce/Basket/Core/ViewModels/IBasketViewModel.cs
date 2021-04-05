using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels
{
    public interface IBasketViewModel : IBaseViewModel
    {
        bool IsEmpty { get; }

        /// <summary>
        /// Список товаров в корзине
        /// </summary>
        ObservableCollection<IBasketItemVM> Items { get; }

        /// <summary>
        /// Выбор товара из списка
        /// </summary>
        ICommand SelectionChangedCommand { get; }

        /// <summary>
        /// Удаление товара (в основном для удаления по свайпу, т.к. нужен доступ с экрана, а не их ячейки)
        /// </summary>
        ICommand DeleteItemCommand { get; }

        /// <summary>
        /// Переход в каталог, когда в корзине пусто
        /// </summary>
        IMvxCommand CatalogCommand { get; }

        /// <summary>
        /// VM для блока лояльности внизу страницы
        /// </summary>
        IMvxViewModel LoyaltyVm { get; }

        /// <summary>
        /// Итоговая сумма корзины
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Доступность перехода к оформлению заказа
        /// </summary>
        bool CanGoNext { get; }

        /// <summary>
        /// Переход к оформлению заказа
        /// </summary>
        IMvxCommand NextCommand { get; }
    }
}
