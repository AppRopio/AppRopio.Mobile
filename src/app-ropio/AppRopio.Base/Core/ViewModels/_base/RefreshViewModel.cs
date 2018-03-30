using System;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Core.ViewModels
{
    public abstract class RefreshViewModel : LoadMoreViewModel, IRefreshViewModel
    {
        #region Fields

        #endregion

        #region Commands

        private IMvxCommand _reloadCommand;
        public IMvxCommand ReloadCommand => _reloadCommand ?? (_reloadCommand = new MvxCommand(OnReloadExecute));

        #endregion

        #region Properties

        private bool _reloading;
        public bool Reloading
        {
            get
            {
                return _reloading;
            }
            set
            {
                _reloading = value;
                RaisePropertyChanged(() => Reloading);
            }
        }

        #endregion

        #region Protected

        protected virtual async void OnReloadExecute()
        {
            if (Reloading)
                return;
            
            Reloading = true;

            await ReloadContent();

            Reloading = false;
        }

        #endregion

        #region Protected

        protected abstract Task ReloadContent();

        #endregion
    }
}
