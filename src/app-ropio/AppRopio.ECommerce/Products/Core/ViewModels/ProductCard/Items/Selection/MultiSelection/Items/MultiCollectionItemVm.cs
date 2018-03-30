using System;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection.Items
{
    public class MultiCollectionItemVM : MvxViewModel
    {
        public string Id { get; private set; }

        public string ValueName { get; private set; }

        public MultiCollectionItemVM(string id, string valueName)
        {
            Id = id;
            ValueName = valueName;
        }
    }
}
