using System;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Basket.Core.Messages.Autocomplete
{
    public class AutocompleteStartMessage : MvxMessage
    {
        public string FieldId { get; set; }

        public string FieldValue { get; set; }

        public AutocompleteStartMessage(object sender)
            : base (sender)
        {
        }
    }
}
