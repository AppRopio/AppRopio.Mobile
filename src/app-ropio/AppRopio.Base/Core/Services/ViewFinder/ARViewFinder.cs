using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Services.ViewLookup;
using MvvmCross.Views;
using AppRopio.Base.Core.Services.ViewModelLookup;
using System.Linq;
namespace AppRopio.Base.Core.Services.ViewFinder
{
    public class ARViewFinder : IMvxViewFinder
    {
        private IViewLookupService _viewLookupService;
        private IViewModelLookupService _viewModelLookupService;

        public ARViewFinder(IViewLookupService viewLookupService, IViewModelLookupService viewModelLookupService)
        {
            _viewModelLookupService = viewModelLookupService;
            _viewLookupService = viewLookupService;
        }

        public Type GetViewType(Type viewModelType)
        {
            var viewModelInterfaceType = _viewModelLookupService.GetInterfaceViewModelLookup().FirstOrDefault(x => x.Value == viewModelType || x.Key == viewModelType).Key;

            return _viewLookupService.Resolve(viewModelInterfaceType ?? viewModelType);
        }
    }
}
