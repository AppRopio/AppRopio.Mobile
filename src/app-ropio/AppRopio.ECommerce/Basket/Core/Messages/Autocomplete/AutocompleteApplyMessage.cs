using System;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Basket.Core.Messages.Autocomplete
{
    public class AutocompleteApplyMessage : MvxMessage
    {
        public string FieldId { get; set; }

        public string FieldValue { get; set; }

        public AutocompleteApplyMessage(object sender)
            : base (sender)
        {
        }
    }
}
