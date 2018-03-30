using System;
using AppRopio.Base.Core.ViewModels.WebContent;

namespace AppRopio.Base.Information.Core.ViewModels.InformationWebContent
{
    public class InformationWebContentViewModel : BaseWebContentViewModel, IInformationWebContentViewModel
    {
		protected override bool CanLoadFinishedExecute(string url)
		{
			return true;
		}

		protected override void OnLoadFinishedExecute(string url)
		{

		}
    }
}