using System;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;

namespace AppRopio.Base.Information.Core.Services
{
    public interface IInformationNavigationVmService : IBaseVmNavigationService
    {
        void NavigateToTextContent(BaseTextContentBundle bundle);

        void NavigateToWebContent(BaseWebContentBundle bundle);

        void NavigateToUrl(BaseWebContentBundle bundle);
    }
}