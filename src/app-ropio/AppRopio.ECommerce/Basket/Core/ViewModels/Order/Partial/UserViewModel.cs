using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial
{
    public class UserViewModel : BaseViewModel, IUserViewModel
    {
        #region Fields

        #endregion

        #region Commands

        private IMvxCommand _nextCommand;
        public IMvxCommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new MvxAsyncCommand(OnNextExecute, () => !Loading));
            }
        }

        private IMvxCommand _selectionChangedCommand;
        public virtual IMvxCommand SelectionChangedCommand => _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IOrderFieldItemVM>(OnUserItemSelected));

        #endregion

        #region Properties

        private MvxObservableCollection<IOrderFieldsGroupVM> _userFieldsGroups;
        public MvxObservableCollection<IOrderFieldsGroupVM> Items
        {
            get
            {
                return _userFieldsGroups;
            }
            set
            {
                _userFieldsGroups = value;
                RaisePropertyChanged(() => Items);
            }
        }

        public decimal BasketAmount { get; protected set; }

        private decimal _amount;
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                RaisePropertyChanged(() => Amount);
            }
        }

        private bool _canGoNext;
        public bool CanGoNext
        {
            get
            {
                return _canGoNext;
            }
            set
            {
                _canGoNext = value;
                RaisePropertyChanged(() => CanGoNext);
            }
        }

        #endregion

        #region Services

        protected IUserVmService UserVmService { get { return Mvx.IoCProvider.Resolve<IUserVmService>(); } }
        protected new IBasketNavigationVmService NavigationVmService { get { return Mvx.IoCProvider.Resolve<IBasketNavigationVmService>(); } }

        #endregion

        #region Constructor

        public UserViewModel()
        {
            VmNavigationType = NavigationType.Push;
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected virtual async Task OnNextExecute()
        {
            if (!await ValidateAndSaveInput(Items))
                return;

            NavigationVmService.NavigateToDeliveryTypes(new BasketBundle(NavigationType.Push, BasketAmount));
        }

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var basketBundle = parameters.ReadAs<BasketBundle>();
            this.InitFromBundle(basketBundle);
        }

        protected virtual void InitFromBundle(BasketBundle basketBundle)
        {
            BasketAmount = basketBundle.BasketAmount;
            Amount = basketBundle.BasketAmount;
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        public virtual async Task LoadContent()
        {
            Loading = true;

            Items = await UserVmService.LoadUserFieldsGroups();

            Loading = false;

            CanGoNext = true;
        }

        public virtual async Task<bool> ValidateAndSaveInput(IEnumerable<IOrderFieldsGroupVM> fieldsGroup)
        {
            Loading = true;

            var isValid = await UserVmService.ValidateAndSaveUserFields(fieldsGroup);

            Loading = false;

            return isValid;
        }


        public virtual void OnUserItemSelected(IOrderFieldItemVM item)
        {
            if (item.Type == AppRopio.Models.Basket.Responses.Enums.OrderFieldType.Counter)
                item.Expanded = !item.Expanded;
        }

        #endregion
    }
}
