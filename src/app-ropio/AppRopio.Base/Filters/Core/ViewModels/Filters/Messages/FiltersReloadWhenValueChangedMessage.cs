using System;
using MvvmCross.Plugins.Messenger;
namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Messages
{
    public class FiltersReloadWhenValueChangedMessage : MvxMessage
    {
        public FiltersReloadWhenValueChangedMessage(object sender)
            : base(sender)
        {
        }
    }
}
