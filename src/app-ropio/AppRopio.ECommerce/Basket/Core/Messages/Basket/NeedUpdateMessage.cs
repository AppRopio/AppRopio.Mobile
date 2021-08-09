using System;
using MvvmCross.Plugin.Messenger;

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
