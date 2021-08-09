using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial
{
    public interface IUserViewModel : IBaseViewModel, IOrderViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

		/// <summary>
		/// Сгруппированный список полей информации о пользователе, получателе и пр.
		/// </summary>
        MvxObservableCollection<IOrderFieldsGroupVM> Items { get; }

        void OnUserItemSelected(IOrderFieldItemVM item);

        Task LoadContent();

        Task<bool> ValidateAndSaveInput(IEnumerable<IOrderFieldsGroupVM> fieldsGroup);
    }
}
