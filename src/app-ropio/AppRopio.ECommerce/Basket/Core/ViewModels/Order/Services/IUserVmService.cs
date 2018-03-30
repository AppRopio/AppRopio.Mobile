using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services
{
    public interface IUserVmService
    {
        Task<MvxObservableCollection<IOrderFieldsGroupVM>> LoadUserFieldsGroups();

        Task<bool> ValidateAndSaveUserFields(IEnumerable<IOrderFieldsGroupVM> fieldsGroups);
    }
}
