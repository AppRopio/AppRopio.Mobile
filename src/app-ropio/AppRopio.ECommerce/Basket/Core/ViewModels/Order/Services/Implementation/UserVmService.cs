using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.Models.Basket.Responses.Order;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services.Implementation
{
    public class UserVmService : BaseVmService, IUserVmService
    {
        #region Protected

        protected virtual IOrderFieldsGroupVM SetupOrderFieldsGroup(OrderFieldsGroup model)
        {
            return new OrderFieldsGroupVM(model);
        }

        #endregion

        #region IUserVmService implementation

        public async Task<MvxObservableCollection<IOrderFieldsGroupVM>> LoadUserFieldsGroups()
        {
            MvxObservableCollection<IOrderFieldsGroupVM> source = null;

            try
            {
                var userFieldsGroups = await Mvx.Resolve<IOrderService>().GetUserFieldsGroups();
                source = new MvxObservableCollection<IOrderFieldsGroupVM>(userFieldsGroups.Select(SetupOrderFieldsGroup));
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return source;
        }

        public async Task<bool> ValidateAndSaveUserFields(IEnumerable<IOrderFieldsGroupVM> fieldsGroups)
        {
            var result = false;

            try
            {
                var fields = fieldsGroups.SelectMany(x => x.Items);

                var validation = await Mvx.Resolve<IOrderService>().ConfirmUser(fields.ToDictionary(x => x.Id, x => x.Value));

                if (validation == null || (validation.NotValidFields.IsNullOrEmpty() && validation.Error.IsNullOrEmtpy()))
                    result = true;
                else
                {
                    foreach (var notValidField in validation.NotValidFields)
                    {
                        var field = fields.FirstOrDefault(x => x.Id.Equals(notValidField.Id));
                        field.IsValid = false;
                    }

                    if (!validation.Error.IsNullOrEmpty())
                        UserDialogs.Error(validation.Error);
                }
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return result;
        }

        #endregion
    }
}
