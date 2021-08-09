using System;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Core.Services.ViewModelNameMapping
{
    public class ARPostfixAwareViewToViewModelNameMapping : MvxViewToViewModelNameMapping
    {
        private readonly string[] _postfixes;

        protected IViewLookupService ViewLookupService { get; }
        protected IViewModelLookupService ViewModelLookupService { get; }

        public ARPostfixAwareViewToViewModelNameMapping(IViewLookupService viewLookupService, IViewModelLookupService viewModelLookupService, params string[] postfixes)
        {
            ViewModelLookupService = viewModelLookupService;
            ViewLookupService = viewLookupService;

            _postfixes = postfixes;
        }

        public override string Map(string inputName)
        {
            if (ViewLookupService.IsRegisteredView(inputName))
            {
                var viewModelInterfaceType = ViewLookupService.ResolveByView(inputName);
                if (ViewModelLookupService.IsRegistered(viewModelInterfaceType))
                    return ViewModelLookupService.Resolve(viewModelInterfaceType).Name;
            }

            foreach (var postfix in _postfixes)
            {
                if (inputName.EndsWith(postfix, StringComparison.Ordinal) && inputName.Length > postfix.Length)
                {
                    inputName = inputName.Substring(0, inputName.Length - postfix.Length);
                    break;
                }
            }
            return base.Map(inputName);
        }
    }
}
