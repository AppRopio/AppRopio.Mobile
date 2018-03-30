using System;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Basket.Core.Messages.Basket
{
    public class NeedUpdateMessage : MvxMessage
    {
        public NeedUpdateMessage(object sender)
            : base (sender)
        {
        }
    }
}
