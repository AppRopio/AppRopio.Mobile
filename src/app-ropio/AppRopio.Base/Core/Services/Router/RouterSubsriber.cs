using System;
using MvvmCross.ViewModels;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;

namespace AppRopio.Base.Core.Services.Router
{
    public abstract class RouterSubsriber : BaseVmNavigationService, IRouterSubscriber
    {
        #region Services

        protected IViewModelLookupService LookupService { get { return Mvx.Resolve<IViewModelLookupService>(); } }

        #endregion

        #region IRouterSubscriber implementation

        public virtual bool CanNavigatedTo(string type, BaseBundle bundle = null)
        {
            if (LookupService.IsRegistered(type))
            {
                NavigateTo(type, bundle);
                return true;
            }

            return false;
        }

        public virtual void FailedNavigatedTo(string type, BaseBundle bundle = null)
        {
            //nothing
        }

        #endregion
    }
}
