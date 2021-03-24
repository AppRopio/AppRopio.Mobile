using System;
using MvvmCross.Plugin.Messenger;
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
