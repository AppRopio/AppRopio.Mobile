using System;
using AppRopio.Base.Core.ViewModels.WebContent;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductWebContent
{
    public class ProductWebContentViewModel : BaseWebContentViewModel, IProductWebContentViewModel
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
