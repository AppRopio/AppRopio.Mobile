using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System.Threading.Tasks;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo.Services;
using MvvmCross.Platform;
using AppRopio.Base.Core.ViewModels;

namespace AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo
{
    public class PromoCodeViewModel : BaseViewModel, IPromoCodeViewModel
    {
        #region Fields

        private string _lastApplyCode;
        private MvxMessage _updateMessage;

        #endregion

        #region Commands

        private IMvxCommand _applyCommand;
        public IMvxCommand ApplyCommand
        {
            get
            {
                return _applyCommand ?? (_applyCommand = new MvxCommand(OnApplyExecute));
            }
        }

        #endregion

        #region Properties

        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                RaisePropertyChanged(() => Code);
            }
        }

        #endregion

        #region Services

        protected IPromoVmService VmService { get { return Mvx.Resolve<IPromoVmService>(); } }
        protected IMvxMessenger Messenger { get { return Mvx.Resolve<IMvxMessenger>(); } }

        #endregion

        #region Constructor

        public PromoCodeViewModel()
        {

        }

        #endregion

        #region Private

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            
        }

        #endregion

        protected virtual void OnApplyExecute()
        {
            if (_lastApplyCode == Code)
                return;

            _lastApplyCode = Code;

            Task.Run(async () => 
            {
                if (await VmService.ApplyPromoCode(Code))
                    Messenger.Publish(_updateMessage);
            });
        }

        #endregion

        #region Public

        public virtual void RegisterUpdateMessage(MvxMessage updateMessage)
        {
            if (_updateMessage != null)
                throw new InvalidOperationException("UpdateMessage type already registered!");

            _updateMessage = updateMessage;
        }

        #endregion
    }
}
